using UnityEngine;

public class LevelManager2D : MonoBehaviour
{
    [SerializeField] private Enemy2D enemyPrefab;
    [SerializeField] private ShipController player;
    
    void Start()
    {
        player = FindFirstObjectByType<ShipController>();
        InvokeRepeating(nameof(SpawnEnemy), 1f, 2.5f);
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(3f, 8f), Random.Range(-4.5f, 4.5f), 0);
        
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<Enemy2D>().player = player.gameObject;
    }
}
