using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarSpawner : MonoBehaviour
{
    static System.Random r;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] carPrefabs;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] Vector2Int spawnAmount;
    private float currentSpawnTime;
    [SerializeField] float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        r = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnTime -= Time.deltaTime;
        if(currentSpawnTime <= 0)
        {
            currentSpawnTime += timeBetweenSpawns;
            Spawn();
        }
    }

    public void Spawn()
    {
        int amountToSpawn = r.Next(spawnAmount.x, spawnAmount.y+1);
        for(int i = 0; i < amountToSpawn; i++)
        {
            int index = r.Next(0,10000000);
            Transform selectedSpawnPoint = spawnPoints[index % spawnPoints.Length];
            if(Physics.OverlapSphere(selectedSpawnPoint.position,5f,LayerMask.GetMask("Car")).Length != 0)
            {
                return;
            }
            GameObject spawnedObject = Instantiate(carPrefabs[r.Next(0,carPrefabs.Length)]);
            spawnedObject.name = "Car " + index;
            spawnedObject.transform.position = selectedSpawnPoint.position;
            spawnedObject.transform.rotation = selectedSpawnPoint.rotation;
            Destroy(spawnedObject, lifetime);
        }
    }
}
