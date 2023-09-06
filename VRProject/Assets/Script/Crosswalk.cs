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
        GameObject newPedestrianPrefab = PedestrianPrefabs[0];
        PedestrianController pedestrianController = newPedestrianPrefab.GetComponent<PedestrianController>();


        pedestrianController.DecidePedestrianMovement(true, true);

        

        Instantiate(newPedestrianPrefab);
        yield return null;

    }



}
