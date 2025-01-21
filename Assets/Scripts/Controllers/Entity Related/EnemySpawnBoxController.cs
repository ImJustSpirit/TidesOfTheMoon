using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnBoxController : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnConfiguration> enemySpawnConfigurations;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("triggered");
        }
    }
}
