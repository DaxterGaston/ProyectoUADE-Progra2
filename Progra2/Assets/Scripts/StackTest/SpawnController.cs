using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    #region EnemySpawning
    [SerializeField] private float enemyRespawnTimerCooldown = 3f; // Tiempo maximo entre cada enemigo
    public PickUp[] enemyArray; // Array de objetos a spawnear
    public List<Transform> spawnEnemyPoints; // Lista de puntos disponibles para spawnear
    private float currentEnemyRespawnTimer;
    private int lastEnemySpawnIndex; // Ultimo punto de spawn
    public LayerMask spawnerEnemyLayerCheck; // Layer utilizada por los enemigos
    public float spawnerEnemyRadiusCheck; // Radio que checkea para spawnear
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
    
    private void Update()
    {
        // currentEnemyRespawnTimer -= Time.deltaTime;
        // if (currentEnemyRespawnTimer <= 0)
        // {
        //     // checkeo pool
        //     SpawnEnemyOverlapCheck();
        // }
        
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
        var nextSpawn = enemyArray[Random.Range(0,enemyArray.Length)]; // Objeto que hay que spawnear
        Instantiate(nextSpawn, spawnEnemyPoints[lastEnemySpawnIndex]);
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
}
