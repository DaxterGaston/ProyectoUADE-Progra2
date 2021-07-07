using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    public enum EnemySpawningMethod
    {
        QueuePool,
        Waves
    };

    [Tooltip("Metodo que utiliza para spawnear enemigos")]
    public EnemySpawningMethod enemySpawnMethod;

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
    [HideInInspector] public bool dontDestroyOnLoad;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
    
            if (dontDestroyOnLoad)
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

    private int KilledAmount;

    #region EnemySpawning

    [Header("EnemyPool Settings")] [SerializeField]
    private float enemyRespawnTimerCooldown = 3f; // Tiempo maximo entre cada enemigo
    private BasePool<EnemyBehaviour> enemyArray; // Array de objetos a spawnear
    public List<Transform> spawnEnemyPoints; // Lista de puntos disponibles para spawnear
    private float currentEnemyRespawnTimer;
    private int lastEnemySpawnIndex; // Ultimo punto de spawn
    public LayerMask spawnerEnemyLayerCheck; // Layer utilizada por los enemigos
    private float spawnerEnemyRadiusCheck = 0.1f; // Radio que checkea para spawnear
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyAmount;

    #endregion

    #region ItemSpawning

    [Header("Item Settings")] [SerializeField]
    private float itemRespawnTimerCooldown = 3f; // Tiempo maximo entre cada item

    [SerializeField] private int maxNumberOfItems = 1;
    private int currentNumberOfItems;
    private float currentItemRespawnTimer;
    private int lastItemSpawnIndex; // Ultimo punto de spawn
    public PickUp[] itemArray; // Array de objetos a spawnear
    public List<Transform> spawnItemPoints; // Lista de puntos disponibles para spawnear
    public LayerMask spawnerItemLayerCheck; // Layer utilizada por los items
    private float spawnerItemRadiusCheck = 0.1f; // Radio que checkea para spawnear

    #endregion

    #region WaveSpawning

    [Header("EnemyWave Settings")] 
    [SerializeField][Tooltip("Numero maximo de waves a derrotar")]
    private int maxNumberOfWaves;

    [SerializeField] public Transform[] dijkstraPathPoints;
    [SerializeField] [Tooltip("INT32 para referenciar dijkstraPathPoints")]
    private int[] dijkstraSpawningPoints;
    private int currentWave = 1;
    private WaveController waveController;

    #endregion

    private void Start()
    {
        switch (enemySpawnMethod)
        {
            case EnemySpawningMethod.QueuePool:
                enemyArray = new BasePool<EnemyBehaviour>();
                InitializePool();
                break;
            case EnemySpawningMethod.Waves:
                waveController = GetComponent<WaveController>();
                waveController.LoadXml(maxNumberOfWaves);
                waveController.WaveSetup(currentWave, maxNumberOfWaves);
                dijkstraSpawningPoints =  new int[]{05,12,25,31,38,45,65,70,72,77,81,90,92,97,106,112,127,130};
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void Update()
    {
        currentEnemyRespawnTimer -= Time.deltaTime;
        if (currentEnemyRespawnTimer <= 0)
        {
            switch (enemySpawnMethod)
            {
                case EnemySpawningMethod.QueuePool:
                    if (enemyArray.HasAvaliable())
                    {
                        SpawnEnemyOverlapCheck();
                    }

                    break;
                case EnemySpawningMethod.Waves:
                    if (!waveController.IsQueueEmpty)
                    {
                        SpawnWaveEnemyOverlapCheck();
                    }
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        currentItemRespawnTimer -= Time.deltaTime;
        if (currentItemRespawnTimer <= 0)
        {
            if (currentNumberOfItems < maxNumberOfItems)
            {
                SpawnItemOverlapCheck();
            }
        }

        if (KilledAmount == 1)
        {
            SceneManager.LoadScene("Win");
        }
    }

    public void StoreEnemy(EnemyBehaviour e)
    {
        KilledAmount++;
        enemyArray.Store(e);
    }

    public void ReduceSpawnedItemCount()
    {
        currentNumberOfItems--;
        currentItemRespawnTimer = itemRespawnTimerCooldown;
    }

    private int DifferentRandomNumber(int index, int arrayCount)
    {
        int num;
        while (true)
        {
            var rNumber = Random.Range(0, arrayCount);
            if (index == rNumber)
            {
                continue;
            }

            num = rNumber;
            break;
        }

        return num;
    }

    private void SpawnEnemyOverlapCheck()
    {
        while (true)
        {
            if (Physics2D.OverlapCircle(spawnEnemyPoints[lastEnemySpawnIndex].position, spawnerEnemyRadiusCheck, spawnerEnemyLayerCheck))
            {
                lastEnemySpawnIndex = DifferentRandomNumber(lastEnemySpawnIndex, spawnEnemyPoints.Count);
                continue;
            }

            if (!Physics2D.OverlapCircle(spawnEnemyPoints[lastEnemySpawnIndex].position, spawnerEnemyRadiusCheck, spawnerEnemyLayerCheck))
            {
                SpawnNextEnemy();
                break;
            }
        }
    }

    private void SpawnWaveEnemyOverlapCheck()
    {
        while (true)
        {
            if (Physics2D.OverlapCircle(dijkstraPathPoints[dijkstraSpawningPoints[lastEnemySpawnIndex]].position, spawnerEnemyRadiusCheck, spawnerEnemyLayerCheck))
            {
                lastEnemySpawnIndex = DifferentRandomNumber(lastEnemySpawnIndex, dijkstraSpawningPoints.Length);
                continue;
            }

            if (!Physics2D.OverlapCircle(dijkstraPathPoints[dijkstraSpawningPoints[lastEnemySpawnIndex]].position, spawnerEnemyRadiusCheck, spawnerEnemyLayerCheck))
            {
                waveController.WaveSpawn(dijkstraPathPoints[dijkstraSpawningPoints[lastEnemySpawnIndex]]);
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
            return;
        }

        //Instantiate(nextSpawn, spawnEnemyPoints[lastEnemySpawnIndex]);
        nextSpawn.transform.position = spawnEnemyPoints[lastEnemySpawnIndex].position;
        currentEnemyRespawnTimer = enemyRespawnTimerCooldown;
    }

    private void SpawnItemOverlapCheck()
    {
        while (true)
        {
            if (Physics2D.OverlapCircle(spawnItemPoints[lastItemSpawnIndex].position, spawnerItemRadiusCheck, spawnerItemLayerCheck))
            {
                lastItemSpawnIndex = DifferentRandomNumber(lastItemSpawnIndex, spawnItemPoints.Count);
                continue;
            }

            if (!Physics2D.OverlapCircle(spawnItemPoints[lastItemSpawnIndex].position, spawnerItemRadiusCheck, spawnerItemLayerCheck))
            {
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
        var gos = new List<EnemyBehaviour>();
        for (int i = 0; i < enemyAmount; i++)
        {
            var go = Instantiate(enemyPrefab);
            go.SetActive(false);
            gos.Add(go.GetComponent<EnemyBehaviour>());
        }

        enemyArray.CreateInitialInstances(gos);
    }
}