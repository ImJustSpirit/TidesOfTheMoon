using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerTransform;
    
    public GameObject melee1_EnemyPrefab;
    public GameObject melee2_EnemyPrefab;
    public GameObject ranged1_EnemyPrefab;
    public GameObject ranged2_EnemyPrefab;

    public GameObject bulletPrefab;
    
    public float initialSpawnRate = 2.5f; // Initial spawn interval in seconds
    public int enemiesPerSpawn = 1;
    private float currentSpawnRate;
    private float timeSinceLastSpawnRateUpdate = 0.0f;
    
    // These variables are for the difficulty to increase every spawnRateIncreaseInterval seconds
    // which reduces the time between spawns by spawnRateMultplier_%
    public float spawnRateIncreaseInterval = 10.0f; // Time (seconds) for spawn rate increase
    public float spawnRateMultiplier = 0.25f; // Spawn rate reduction multiplier (50%)

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        InvokeRepeating(nameof(SpawnRangedEnemies), 0, currentSpawnRate);
        InvokeRepeating(nameof(SpawnMeleeEnemies), 0, currentSpawnRate);
    }

    void Update()
    {
        // Gradually increase spawn rate over time
        timeSinceLastSpawnRateUpdate += Time.deltaTime;
        if (timeSinceLastSpawnRateUpdate >= spawnRateIncreaseInterval)
        {
            timeSinceLastSpawnRateUpdate = 0.0f;
            UpdateSpawnRate();
        }
    }

    void SpawnRangedEnemies()
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            // Spawn the enemy prefab at a random position
            Vector3 spawnPosition = new Vector3(Random.Range(-40, 40), Random.Range(-20, 20), playerTransform.position.z + 50);
            rangedEnemy newRangedEnemyClass = Instantiate(ranged1_EnemyPrefab, spawnPosition, Quaternion.identity).GetComponent<rangedEnemy>();

            // Assigning Enemy variables
            newRangedEnemyClass.collisionDamage = 10;
            newRangedEnemyClass.playerTransform = playerTransform;

            // Assigning rangedEnemy variables
            newRangedEnemyClass.shootRate = 1f;
            newRangedEnemyClass.bulletSpeed = 30f;
            newRangedEnemyClass.bulletDamage = 5f;
            newRangedEnemyClass.bulletPrefab = bulletPrefab;

        }
    }

    void SpawnMeleeEnemies()
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            // Spawn the enemy prefab at a random position
            Vector3 spawnPosition = new Vector3(Random.Range(-40, 40), Random.Range(-20, 20), playerTransform.position.z + 50);
            meleeEnemy newMeleeEnemyClass = Instantiate(melee2_EnemyPrefab, spawnPosition, Quaternion.identity).GetComponent<meleeEnemy>();

            // Assigning Enemy variables
            newMeleeEnemyClass.collisionDamage = 5;
            newMeleeEnemyClass.playerTransform = playerTransform;
        }
    }

    void UpdateSpawnRate()
    {
        // Cancel the current repeating invocation
        CancelInvoke(nameof(SpawnRangedEnemies));
        CancelInvoke(nameof(SpawnMeleeEnemies));

        // Reduce the spawn interval by the multiplier (i.e., increase spawn rate)
        currentSpawnRate *= spawnRateMultiplier;

        // Restart the invocation with the updated spawn interval
        InvokeRepeating(nameof(SpawnRangedEnemies), 0, currentSpawnRate);
        InvokeRepeating(nameof(SpawnMeleeEnemies), 0, currentSpawnRate);
    }
}