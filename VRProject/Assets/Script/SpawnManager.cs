using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawner")]
    public GameObject carPrefab;
    public Transform spOne;
    public Transform spTwo;
    public Transform spThree;
    public Transform spFour;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(carPrefab, spOne.position, spOne.rotation);
        Instantiate(carPrefab, spTwo.position, spTwo.rotation);
        Instantiate(carPrefab, spThree.position, spThree.rotation);
        Instantiate(carPrefab, spFour.position, spFour.rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
