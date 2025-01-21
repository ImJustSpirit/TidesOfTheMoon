using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemySpawnBoxController : MonoBehaviour
{
    [Header ("Enemy Spawn Configurations")]
    [SerializeField] private List<EnemySpawnConfiguration> enemySpawnConfigurations;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject ranged1;
    [SerializeField] private GameObject ranged2;
    [SerializeField] private GameObject melee1;
    [SerializeField] private GameObject melee2;
    
    [Header("Dev Variable")]
    [SerializeField] private bool showAreaBounds = false;

    private GameObject enemyPrefabObject;
    
    private Transform playerTransform;
    [SerializeField] private GameObject bulletPrefab;

    void Start()
    {
        // Oliver - Mildly inefficient but I think this is okay
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (!showAreaBounds)
        {
            GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Attempting Enemy Spawns");
            
            Bounds enemySpawnAreaBounds = transform.GetChild(0).GetComponent<MeshRenderer>().bounds;

            for (int i = 0; i < enemySpawnConfigurations.Count; i++)
            {
                EnemySpawnConfiguration currentEnemyConfiguration = enemySpawnConfigurations[i];

                // Checks to make sure parameters are valid
                if (currentEnemyConfiguration.amount <= 0) continue;
                if (currentEnemyConfiguration.amount > 100 && currentEnemyConfiguration.sequentialDelay < 0.01f) // CHANGE OR REMOVE THIS if you want to die
                {
                    Debug.LogError("sorry but I'm NOT letting you spawn " + currentEnemyConfiguration.amount + " fucking enemies at once");
                    continue;
                }
                if (currentEnemyConfiguration.sequentialDelay > 30 && currentEnemyConfiguration.spawnSequentially)
                {
                    Debug.LogWarning("sequentialDelay is very high, enemies may spawn behind the player");
                }

                if (currentEnemyConfiguration.spawnSequentially &&
                    currentEnemyConfiguration.sequentialDelay > 0)
                {
                    // Use a coroutine to spawn enemies
                    StartCoroutine(SpawnEnemiesCoroutine(currentEnemyConfiguration.amount,
                        currentEnemyConfiguration.sequentialDelay, currentEnemyConfiguration.enemyType.ToString(), enemySpawnAreaBounds));
                }
                else
                {
                    // Spawn enemies instantly if not sequential
                    for (int x = 0; x < currentEnemyConfiguration.amount; x++)
                    {
                        SpawnEnemy(currentEnemyConfiguration.enemyType.ToString(), enemySpawnAreaBounds);
                    }
                }
            }
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(int amount, float delay, string enemyPrefab, Bounds enemyBounds)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnEnemy(enemyPrefab, enemyBounds);
            yield return new WaitForSeconds(delay); // Wait for specified delay
        }
    }
    
    private void SpawnEnemy(string enemyPrefab, Bounds enemyBounds)
    {
        switch (enemyPrefab)
        {
            case "ranged1":
                enemyPrefabObject = ranged1;
                break;
            case "ranged2":
                enemyPrefabObject = ranged2;
                break;
            case "melee1":
                enemyPrefabObject = melee1;
                break;
            case "melee2":
                enemyPrefabObject = melee2;
                break;
        }
        
        if (enemyPrefab == null) return;

        // You can use bounds or define spawn logic here
        Vector3 spawnPosition = new Vector3(
            Random.Range(enemyBounds.min.x, enemyBounds.max.x), // Random X Within Bounds
            Random.Range(enemyBounds.min.y, enemyBounds.max.y), // Random Y Within Bounds
            enemyBounds.center.z);                              // Z Centered Within Bounds


        Enemy newEnemyClass = Instantiate(enemyPrefabObject, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        newEnemyClass.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (newEnemyClass is rangedEnemy)
        {
            newEnemyClass.GetComponent<rangedEnemy>().bulletPrefab = bulletPrefab;
        }
        
    }
    
    /*private GameObject GetEnemyPrefab(string enemyType)
    {
        // Logic to fetch the correct prefab
        return enemyType switch
        {
            "Ranged1" => ranged1,
            "Ranged2" => ranged2,
            "Melee1" => melee1,
            "Melee2" => melee2,
            _ => null,
        };
    }*/
}
