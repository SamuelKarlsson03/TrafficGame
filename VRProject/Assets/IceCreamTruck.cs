using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamTruck : MonoBehaviour
{

    [SerializeField] GameObject iceCreamCone;

    Car carScript;

    float timer = 0f;
    float timeToCheckIfCrashed = 0.5f;

    private void Start()
    {
        carScript = GetComponent<Car>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToCheckIfCrashed)
        {
            timer = 0f;
            if (carScript.)
            {

            }
        }

    }

    private void SpawnIceCreamCones()
    {

    }

}
