using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamTruck : MonoBehaviour
{

    [SerializeField] GameObject iceCreamCone;
    [SerializeField] Transform coneSpawnPos;

    Car carScript;

    float timer = 0f;
    [SerializeField] float timeToCheckIfCrashed = 0.5f;

    [SerializeField] float timeBetweenIceCreamCone = 0.025f;
    [SerializeField] float amountOfIceCreamCones = 100f;

    private void Start()
    {
        carScript = GetComponent<Car>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SpawnIceCreamCones());
        }

        timer += Time.deltaTime;

        //Checks if the ice cream truck has crashed 2 times every second
        if (timer >= timeToCheckIfCrashed)
        {
            timer = 0f;
            if (carScript.broken)
            {
                StartCoroutine(SpawnIceCreamCones());
            }
        }

    }

    //Shoots out ice cream cones when crashed
    IEnumerator SpawnIceCreamCones()
    {
        for (int i = 0; i < amountOfIceCreamCones; i++)
        {
            Instantiate(iceCreamCone, coneSpawnPos.position, coneSpawnPos.rotation);

            yield return new WaitForSeconds(timeBetweenIceCreamCone);
        }

        yield return null;
    }

}
