using UnityEngine;

[System.Serializable]
public class EnemySpawnConfiguration
{
    public enum EnemyType
    {
        ranged1,
        melee2
    }

    public EnemyType enemyType;
    public int amount;

    public bool spawnSequentially;
    public float sequentialDelay;
}
