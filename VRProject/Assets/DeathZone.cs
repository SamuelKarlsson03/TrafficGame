using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pedestrian"))
        {
            other.gameObject.GetComponent<PedestrianController>().CheckIfDie();
        }
    }


}
