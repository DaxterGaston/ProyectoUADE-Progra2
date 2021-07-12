using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    // Enum de seleccion de tipo de spawn
    public enum EnemySpawningMethod { QueuePool, Waves };

    [Tooltip("Metodo que utiliza para spawnear enemigos")]
    public EnemySpawningMethod enemySpawnMethod; // Selector de tipo de spawn

    #region ConstructorSingleton

    // private static SpawnController _instance;
    //  private SpawnController()
    //  {
    //      Start();
    //  }
    //  public static SpawnController Instance
    //  {
    //      get
    //      {
    //          if (_instance == null)
    //          {
    //              _instance = new SpawnController();
    //          }
    //
    //          return _instance;
    //      }
    //  }

    #endregion

    #region AwakeSingleton

    public static SpawnController Instance;
    [HideInInspector] public bool doNotDestroyOnLoad;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
    
            if (doNotDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    
    #region EnemySpawning

    [Header("EnemyPool Settings")] [SerializeField]
    private float enemyRespawnTimerCooldown = 3f; // Tiempo maximo entre cada enemigo
    private BasePool<EnemyController> enemyArray; // Array de objetos a spawnear
    public List<Transform> spawnEnemyPoints; // Lista de puntos disponibles para spawnear
    private float currentEnemyRespawnTimer;
    private int lastEnemySpawnIndex; // Ultimo punto de spawn
    public LayerMask spawnerEnemyLayerCheck; // Layer utilizada por los enemigos
    private float spawnerEnemyRadiusCheck = 0.1f; // Radio que checkea para spawnear
    [SerializeField] private GameObject enemyPrefab; // Prefab que va a spawnear
    [SerializeField] private int enemyAmount; // Cantidad de enemigos para inicializar el pool

    #endregion

    #region ItemSpawning

    [Header("Item Settings")] [SerializeField]
    private float itemRespawnTimerCooldown = 3f; // Tiempo maximo entre cada item

    [SerializeField] private int maxNumberOfItems = 1; // Maximo numero de items a la vez
    private int currentNumberOfItems; // Cantidad actual de enemigos
    private float currentItemRespawnTimer; // Cooldown actual para respawn
    private int lastItemSpawnIndex; // Ultimo punto de spawn
    public PickUp[] itemArray; // Array de objetos a spawnear
    public List<Transform> spawnItemPoints; // Lista de puntos disponibles para spawnear
    public LayerMask spawnerItemLayerCheck; // Layer utilizada por los items
    private float spawnerItemRadiusCheck = 0.1f; // Radio que checkea para spawnear

    #endregion

    #region WaveSpawning

    [Header("EnemyWave Settings")] 
    [SerializeField][Tooltip("Numero maximo de waves a derrotar")]
    private int maxNumberOfWaves; // Numero maximo de waves que genera el documento xml
    [SerializeField] public Transform[] dijkstraPathPoints; // Array que incluye todos los puntos del grafo
    [SerializeField] [Tooltip("INT32 para referenciar dijkstraPathPoints")]
    private int[] dijkstraSpawningPoints; // Int usados para referenciar los puntos del grafo
    private int currentWave = 1; // Wave actual
    private WaveController waveController; // Referencia al controlador de wave

    #endregion

    #region Win Variables

    public static int killedAmount; // Enemigos derrotados

    public void KillUpdate()
    {
        OnKillConfirmed?.Invoke();
    }
    
    [SerializeField] private int queueWinKilledAmount = 10; // Cantidad necesaria de enemigos derrotados necesaria para ganar
    public int QueuePoolObjective => queueWinKilledAmount; 
        
    #endregion

    #region HUD

    public event Action OnKillConfirmed;
    
    #endregion

    private void Start()
    {
        // Switch para que no haga las cosas que no le corresponden
        switch (enemySpawnMethod)
        {
            // Si es spawn por QueuePool
            case EnemySpawningMethod.QueuePool:
                // Crea un pool donde se guardan los enemigos
                enemyArray = new BasePool<EnemyController>();
                // Inicializa el pool con valores correspondientes
                InitializePool();
                break;
            // Si es spawn por Waves
            case EnemySpawningMethod.Waves:
                // Guarda la referencia del controlador de waves
                waveController = GetComponent<WaveController>();
                // Activa el metodo para cargar el XML con el numero de waves que va a tener
                waveController.LoadXml(maxNumberOfWaves);
                // Prepara la primer wave
                waveController.WaveSetup(currentWave, maxNumberOfWaves);
                // Guarda las referencia a donde pueden spawnear los enemigos
                dijkstraSpawningPoints =  new []{05,12,25,31,38,45,65,70,72,77,81,90,92,97,106,112,127,130};
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void Update()
    {
        // Timer para saber si tiene que spawnear el siguiente enemigo
        currentEnemyRespawnTimer -= Time.deltaTime;
        if (currentEnemyRespawnTimer <= 0)
        {
            // Usa el switch segun el tipo de spawn
            switch (enemySpawnMethod)
            {
                // En el caso de QueuePool
                case EnemySpawningMethod.QueuePool:
                    // Se fija si el pool tiene objetos disponibles
                    if (enemyArray.HasAvaliable())
                    {
                        // Comprueba que no encuentre algo que obstruya el camino donde quiere spawnear
                        SpawnEnemyOverlapCheck();
                    }

                    break;
                // En el caso de Waves
                case EnemySpawningMethod.Waves:
                    // Si la cola de enemigos no esta vacia
                    if (!waveController.IsQueueEmpty)
                    {
                        // Comprueba que no haya obstrucciones en el spawn point
                        SpawnWaveEnemyOverlapCheck();
                    }
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        // Timer para saber si tiene que spawnear el siguiente item
        currentItemRespawnTimer -= Time.deltaTime;
        if (currentItemRespawnTimer <= 0)
        {
            // Si hay menos items del numero maximo
            if (currentNumberOfItems < maxNumberOfItems)
            {
                // Comprueba el lugar de spawn para que no ponga dos en el mismo lugar
                SpawnItemOverlapCheck();
            }
        }
        // Comprueba la win conditions
        WinConditions();
    }

    public void StoreEnemy(EnemyController e)
    {
        killedAmount++;
        enemyArray.Store(e);
    }

    public void ReduceSpawnedItemCount()
    {
        // Reduce el numero actual de items en 1
        currentNumberOfItems--;
        // Reinicia el cooldown
        currentItemRespawnTimer = itemRespawnTimerCooldown;
    }

    private int DifferentRandomNumber(int index, int arrayCount)
    {
        // Variable que va a cambiar
        int num;
        while (true)
        {
            // Numero que genera aleatoriamente en base a los parametros dados
            var rNumber = Random.Range(0, arrayCount);
            // Si el numero generado es el mismo al que vino como parametro
            if (index == rNumber)
            {
                // Sigo generando numeros
                continue;
            }
            // Si el numero es diferente al que vino como parametro lo guardo
            num = rNumber;
            break;
        }
        // Devuelvo el nuevo numero que es diferente al que vino como parametro
        return num;
    }

    private void SpawnEnemyOverlapCheck()
    {
        while (true)
        {
            // Comprueba si hay algo en la posicion indicada
            if (Physics2D.OverlapCircle(spawnEnemyPoints[lastEnemySpawnIndex].position, spawnerEnemyRadiusCheck, spawnerEnemyLayerCheck))
            {
                // Como habia algo tengo que cambiar la posicion indicada a una diferente hasta que encuentre una vacia
                lastEnemySpawnIndex = DifferentRandomNumber(lastEnemySpawnIndex, spawnEnemyPoints.Count);
                continue;
            }
            // Comprueba que la posicion este vacia
            if (!Physics2D.OverlapCircle(spawnEnemyPoints[lastEnemySpawnIndex].position, spawnerEnemyRadiusCheck, spawnerEnemyLayerCheck))
            {
                // Intenta spawnear el enemigo
                SpawnNextEnemy();
                break;
            }
        }
    }

    private void SpawnWaveEnemyOverlapCheck()
    {
        while (true)
        {
            // Se fija si hay algo donde quiere spawnear
            if (Physics2D.OverlapCircle(dijkstraPathPoints[dijkstraSpawningPoints[lastEnemySpawnIndex]].position, spawnerEnemyRadiusCheck, spawnerEnemyLayerCheck))
            {
                // Como habia algo en el spawn point, lo cambia hasta que sea uno diferente
                lastEnemySpawnIndex = DifferentRandomNumber(lastEnemySpawnIndex, dijkstraSpawningPoints.Length);
                continue;
            }
            // Comprueba que no haya nadie donde quiere spawnear
            if (!Physics2D.OverlapCircle(dijkstraPathPoints[dijkstraSpawningPoints[lastEnemySpawnIndex]].position, spawnerEnemyRadiusCheck, spawnerEnemyLayerCheck))
            {
                // Spawnea el siguiente objeto de la queue
                waveController.WaveSpawn(dijkstraPathPoints[dijkstraSpawningPoints[lastEnemySpawnIndex]]);
                // Reinicia el cooldown
                currentEnemyRespawnTimer = enemyRespawnTimerCooldown;
                break;
            }
        }
    }

    private void SpawnNextEnemy()
    {
        var nextSpawn = enemyArray.Get(); // Objeto que hay que spawnear
        if (nextSpawn == null)
        {
            // Si no hay objetos no hace nada
            return;
        }

        //Instantiate(nextSpawn, spawnEnemyPoints[lastEnemySpawnIndex]);
        nextSpawn.transform.position = spawnEnemyPoints[lastEnemySpawnIndex].position;
        // Reinicia el cooldown para spawnear
        currentEnemyRespawnTimer = enemyRespawnTimerCooldown;
    }

    private void SpawnItemOverlapCheck()
    {
        while (true)
        {
            // Se fija si hay algo donde quiere spawnear
            if (Physics2D.OverlapCircle(spawnItemPoints[lastItemSpawnIndex].position, spawnerItemRadiusCheck, spawnerItemLayerCheck))
            {
                // Cambia el lugar de spawn hasta que encuentre uno vacio
                lastItemSpawnIndex = DifferentRandomNumber(lastItemSpawnIndex, spawnItemPoints.Count);
                continue;
            }
            // Se fija si esta vacio
            if (!Physics2D.OverlapCircle(spawnItemPoints[lastItemSpawnIndex].position, spawnerItemRadiusCheck, spawnerItemLayerCheck))
            {
                // Trata de spawnear el siguiente item
                SpawnNextItem();
                break;
            }
        }
    }

    private void SpawnNextItem()
    {
        var nextItem = itemArray[Random.Range(0, itemArray.Length)];
        Instantiate(nextItem, spawnItemPoints[lastItemSpawnIndex]);
        currentNumberOfItems++;
        currentItemRespawnTimer = itemRespawnTimerCooldown;
    }

    private void InitializePool()
    {
        // Genera una lista auxiliar
        var gos = new List<EnemyController>();
        // Recorre el for con el numero maximo que se le puso como variable
        for (int i = 0; i < enemyAmount; i++)
        {
            // aux para guardar el objeto a instanciar
            var go = Instantiate(enemyPrefab);
            // apaga el objeto
            go.SetActive(false);
            // agrega el objeto a la lista
            gos.Add(go.GetComponent<EnemyController>());
        }
        // Genera el pool con los objetos de la lista
        enemyArray.CreateInitialInstances(gos);
    }

    private void WinConditions()
    {
        // Decide la condicion de victoria segun el tipo de spawn
        switch (enemySpawnMethod)
        {
            // En el caso de QueuePool
            case EnemySpawningMethod.QueuePool:
                // Comprueba si mato cierta cantidad de enemigos
                if (killedAmount == queueWinKilledAmount)
                {
                    SceneManager.LoadScene("Win");
                }
                break;
            // En el caso de Waves
            case EnemySpawningMethod.Waves:
                // Comprueba si mato cierta cantidad de enemigos
                if (killedAmount >= waveController.WaveTotalEnemies)
                {
                    // Aumento la wave en 1
                    currentWave++;
                    // Si el numero de la nueva wave es menor o igual al maximo de waves
                    if (currentWave <= maxNumberOfWaves)
                    {
                        // Reinicia la cantidad de enemigos derrotados
                        killedAmount = 0;
                        // se puede poner algo en pantalla que diga wave completada o algo aca ¯\_(ツ)_/¯
                        // Prepara la siguente wave
                        waveController.WaveSetup(currentWave, maxNumberOfWaves);
                    }
                    // Si el numero supera el maximo es que supere todas las waves
                    if (currentWave > maxNumberOfWaves)
                    {
                        currentWave = 1;
                        killedAmount = 0;
                        // Cargo la escena de win
                        SceneManager.LoadScene("Win");
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}