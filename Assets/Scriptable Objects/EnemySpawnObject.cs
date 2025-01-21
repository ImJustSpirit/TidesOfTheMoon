using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnObject", menuName = "Scriptable Objects/EnemySpawnObject")]
public class EnemySpawnObject : ScriptableObject
{
    public EnemySpawnConfiguration[] EnemySpawnConfigurations;
}
