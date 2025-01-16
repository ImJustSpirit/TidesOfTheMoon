using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject melee1_EnemyPrefab;
    public float initialSpawnRate = 2.0f; // Initial spawn interval in seconds
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
            Vector3 spawnPosition = new Vector3(Random.Range(-40, 40), Random.Range(-20, 20), 150);
            GameObject enemy = Instantiate(melee1_EnemyPrefab, spawnPosition, Quaternion.identity);

            // Set the enemy's linear velocity to Vector3.back * speed (read from its Enemy component)
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.back * enemyComponent.speed;
                }
            }
        }
    }

    void UpdateSpawnRate()
    {
        // Cancel the current repeating invocation
        CancelInvoke(nameof(SpawnRangedEnemies));

        // Reduce the spawn interval by the multiplier (i.e., increase spawn rate)
        currentSpawnRate *= spawnRateMultiplier;

        // Restart the invocation with the updated spawn interval
        InvokeRepeating(nameof(SpawnRangedEnemies), 0, currentSpawnRate);
    }
}