using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    #region EnemySpawning
    [SerializeField] private float enemyRespawnTimerCooldown = 3f; // Tiempo maximo entre cada enemigo
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
    [SerializeField] private float itemRespawnTimerCooldown = 3f; // Tiempo maximo entre cada item
    [SerializeField] private int maxNumberOfItems = 1;
    private int currentNumberOfItems;
    private float currentItemRespawnTimer;
    private int lastItemSpawnIndex; // Ultimo punto de spawn
    public PickUp[] itemArray; // Array de objetos a spawnear
    public List<Transform> spawnItemPoints; // Lista de puntos disponibles para spawnear
    public LayerMask spawnerItemLayerCheck; // Layer utilizada por los items
    private float spawnerItemRadiusCheck = 0.1f; // Radio que checkea para spawnear
    #endregion

    private void Start()
    {
        enemyArray = new BasePool<EnemyBehaviour>();
        InitializePool();
    }

    private void Update()
    {
        currentEnemyRespawnTimer -= Time.deltaTime;
        if (currentEnemyRespawnTimer <= 0)
        {
            if (enemyArray.HasAvaliable())
            {
                SpawnEnemyOverlapCheck();
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
                lastEnemySpawnIndex = DifferentRandomNumber(lastEnemySpawnIndex,spawnEnemyPoints.Count);
                continue;
            }

            if (!Physics2D.OverlapCircle(spawnEnemyPoints[lastEnemySpawnIndex].position,spawnerEnemyRadiusCheck,spawnerEnemyLayerCheck))
            {
                SpawnNextEnemy();
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
                lastItemSpawnIndex = DifferentRandomNumber(lastItemSpawnIndex,spawnItemPoints.Count);
                continue;
            }

            if (!Physics2D.OverlapCircle(spawnItemPoints[lastItemSpawnIndex].position,spawnerItemRadiusCheck,spawnerItemLayerCheck))
            {
                SpawnNextItem();
                break;
            }
        }
    }
    private void SpawnNextItem()
    {
        var nextItem = itemArray[Random.Range(0,itemArray.Length)];
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
