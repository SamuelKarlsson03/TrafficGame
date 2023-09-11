using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswalkManager : MonoBehaviour
{
    [SerializeField] GameObject posOne;
    [SerializeField] GameObject posTwo;
    [SerializeField] GameObject posMiddle;

    [Header("Spawn Event stuff")]
    [SerializeField] List<CrosswalkEvent> crosswalkEvents;
    [SerializeField] float randomEventChance = 0.10f;

    [Header("Spawn Variables")]
    [SerializeField] List<GameObject> PedestrianPrefabs;
    [SerializeField] float timer = 0f;
    [SerializeField] float timeUntilSpawn = 30f;
    [SerializeField] float minTimeSpawn = 10f;
    [SerializeField] float maxTimeSpawn = 30f;
    [SerializeField] Vector3 spawnPos;


    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeUntilSpawn)
        {
            PreparePedestrianSpawn();
        }
    }

    private void PreparePedestrianSpawn()
    {
        timer = 0f;
        timeUntilSpawn = Random.Range(minTimeSpawn, maxTimeSpawn);
        StartCoroutine(SpawnPedestrian(Random.Range(1, 5)));
    }

    IEnumerator SpawnPedestrian(int amountSpawned)
    {
        float eventRandomValue = Random.Range(0f, 1f);

        if (eventRandomValue <= randomEventChance)
        {
            PrepareRandomEvent();

        }
        else
        {

            for (int i = 0; i < amountSpawned; i++)
            {

                bool randomBool = Random.Range(0, 2) == 1;

                GameObject newPedestrianPrefab = Instantiate(PedestrianPrefabs[Random.Range(0, PedestrianPrefabs.Count)], transform.position, transform.rotation);
                PedestrianController pedestrianController = newPedestrianPrefab.GetComponent<PedestrianController>();


                if (randomBool) //decides pedestrian direction
                {
                    newPedestrianPrefab.transform.position = posOne.transform.position;
                }
                else
                {
                    newPedestrianPrefab.transform.position = posTwo.transform.position;
                }

                StartPedestrianMovement(pedestrianController);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.15f));

            }

        }

    }

    IEnumerator StartEvent(List<GameObject> pedestriansToSpawn, bool spawnOnSameSide, float time)
    {

        if (spawnOnSameSide)
        {
            bool whatSideBool = Random.Range(0, 2) == 1;
            if (whatSideBool)
            {
                spawnPos = posOne.transform.position;
            }
            else
            {
                spawnPos = posTwo.transform.position;
            }
        }

        for (int i = 0; i < pedestriansToSpawn.Count; i++)
        {

            GameObject newPedestrianPrefab = Instantiate(pedestriansToSpawn[0], transform.position, transform.rotation);
            pedestriansToSpawn.Remove(pedestriansToSpawn[0]);
            PedestrianController pedestrianController = newPedestrianPrefab.GetComponent<PedestrianController>();

            if (!spawnOnSameSide)
            {
                bool randomBool = Random.Range(0, 2) == 1;

                if (randomBool) //decides pedestrian direction 
                {
                    spawnPos = posOne.transform.position;
                    newPedestrianPrefab.transform.position = spawnPos;
                }
                else
                {
                    spawnPos = posTwo.transform.position;
                    newPedestrianPrefab.transform.position = spawnPos;
                }
            }
            else
            {
                newPedestrianPrefab.transform.position = spawnPos;
            }
            Debug.Log("Spawned");
            StartPedestrianMovement(pedestrianController);
            yield return new WaitForSeconds(time);
        }

        yield return null;
    }


    private void StartPedestrianMovement(PedestrianController pedestrianController)
    {
        pedestrianController.StartMovement(posMiddle.transform);
        pedestrianController.transform.LookAt(posMiddle.transform);
    }

    private void PrepareRandomEvent()
    {
        CrosswalkEvent currentEvent = crosswalkEvents[Random.Range(0, crosswalkEvents.Count)];
        List<GameObject> pedestrians = new List<GameObject>();

        pedestrians.AddRange(currentEvent.pedestriansToSpawn);

        bool spawnOnSameSide = currentEvent.spawnOnSameSide;
        float timeBetweenSpawn = currentEvent.timeBetweenSpawn;

        StartCoroutine(StartEvent(pedestrians, spawnOnSameSide, timeBetweenSpawn));
    }


}
