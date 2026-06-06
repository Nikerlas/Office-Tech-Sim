using System.Collections;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;

    public Transform spawnPoint;

    void Start()
    {
        StartCoroutine(
            SpawnRoutine()
        );
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime =
                Random.Range(5f, 10f);

            yield return new WaitForSeconds(
                waitTime
            );

            int randomIndex =
                Random.Range(
                    0,
                    carPrefabs.Length
                );

            Instantiate(
                carPrefabs[randomIndex],
                spawnPoint.position,
                spawnPoint.rotation
            );
        }
    }
}