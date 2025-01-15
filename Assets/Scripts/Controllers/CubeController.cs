using UnityEngine;

public class CubeController : MonoBehaviour
{
    public GameObject cubePrefab;

    public bool Imafraidivediedbutitsokayiwasheavilypregnantsoimnowspawningtheguyokhereheisgoodluckmandontletmedownwewillwinagainsttheseangryrobotships = false;

    private void Update()
    {
        transform.localScale = new Vector3(GetComponent<Health>().health / 5f, GetComponent<Health>().health / 5f, GetComponent<Health>().health / 5f);

        if(GetComponent<Health>().health < 100)
        {
            GetComponent<Health>().health += Time.deltaTime * 4;
        }
    }
}
