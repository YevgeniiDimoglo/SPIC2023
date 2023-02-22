using UnityEngine;

public class RandomManager : MonoBehaviour
{
    private Vector3 off = new Vector3(0, 0, 0.4f);
    // Start is called before the first frame update
    void Start()
    {
        GameObject [] paintings = GameObject.FindGameObjectsWithTag("Paint");

        foreach (var item in paintings)
        {
            item.transform.RotateAround(item.transform.position + off, item.transform.forward, Random.Range(-15.0f, 15.0f));
        }

        GameObject[] randomPlace = GameObject.FindGameObjectsWithTag("RandomPlace");

        GameObject[] holdables = GameObject.FindGameObjectsWithTag("Holdable");

        foreach (var item in holdables)
        {
            var place = Random.Range(0, randomPlace.Length -1);
            Vector3 randomSpawnPlace = new Vector3(randomPlace[place].transform.position.x + Random.Range(-1, 1), 0, randomPlace[place].transform.position.z + Random.Range(-1, 1));
            item.transform.position = randomSpawnPlace;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
