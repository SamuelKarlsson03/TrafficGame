using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] carsToSpawn;
    public Transform[] spawnPositions;

    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 10f;
    private float Timer;

    // Start is called before the first frame update
    void Start()
    {
        Timer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            SpawnCarsRandomly();

            Timer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }

        Debug.Log(Timer);

    }

    void SpawnCarsRandomly()
    {
        foreach (Transform spawnPoint in spawnPositions)
        {
            int randomIndex = Random.Range(0, carsToSpawn.Length);
            GameObject objectToInstantiate = carsToSpawn[randomIndex];
            Instantiate(objectToInstantiate, spawnPoint.position, spawnPoint.rotation);
        }
    }

}
