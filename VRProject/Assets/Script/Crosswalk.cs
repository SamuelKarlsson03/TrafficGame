using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosswalk : MonoBehaviour
{
    [SerializeField] GameObject posOne;
    [SerializeField] GameObject posTwo;

    [SerializeField] List<GameObject> PedestrianPrefabs;

    [SerializeField] float spawnFreq = 10f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SpawnPedestrian());
        }
    }

    IEnumerator SpawnPedestrian()
    {

        bool randomBool = Random.Range(0, 2) == 1; //randomizes bool between true and false

        GameObject newPedestrianPrefab = Instantiate(PedestrianPrefabs[0], transform.position, transform.rotation);
        PedestrianController pedestrianController = newPedestrianPrefab.GetComponent<PedestrianController>();


        if (randomBool) //decides if pedestrians should move from left to right or right to left
        {
            newPedestrianPrefab.transform.position = posOne.transform.position;
            pedestrianController.StartMovement(posOne.transform, posTwo.transform);
        }
        else
        {
            newPedestrianPrefab.transform.position = posTwo.transform.position;
            pedestrianController.StartMovement(posTwo.transform, posOne.transform);
        }

        yield return null;

    }



}
