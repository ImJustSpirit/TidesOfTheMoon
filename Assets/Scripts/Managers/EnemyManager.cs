using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject melee1_EnemyPrefab;
    public float initialSpawnRate = 2.0f; // Initial spawn interval in seconds
    private float currentSpawnRate;
    public float spawnRateIncreaseInterval = 10.0f; // Time (seconds) for spawn rate increase
    public float spawnRateMultiplier = 0.25f; // Spawn rate reduction multiplier (50%)
    private float timeSinceLastSpawnRateUpdate = 0.0f;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        InvokeRepeating(nameof(SpawnRangedEnemy), 0, currentSpawnRate);
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

    void SpawnRangedEnemy()
    {
        // Spawn the enemy prefab at a random position
        Vector3 spawnPosition = new Vector3(Random.Range(-4, 4), Random.Range(-2, 2), 100);
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

    void UpdateSpawnRate()
    {
        // Cancel the current repeating invocation
        CancelInvoke(nameof(SpawnRangedEnemy));

        // Reduce the spawn interval by the multiplier (i.e., increase spawn rate)
        currentSpawnRate *= spawnRateMultiplier;

        // Restart the invocation with the updated spawn interval
        InvokeRepeating(nameof(SpawnRangedEnemy), 0, currentSpawnRate);
    }
}