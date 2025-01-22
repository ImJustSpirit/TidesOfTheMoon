using UnityEngine;

public class RockManager : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;
    private Vector3 randomPosition;
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            randomPosition = new Vector3(
                Random.Range(-1000f, 1000f), 
                Random.Range(-1000f, 1000f),
                Random.Range(-1000f, 1000f));
            Instantiate(rockPrefab, randomPosition, Random.rotation);
        }
    }
}
