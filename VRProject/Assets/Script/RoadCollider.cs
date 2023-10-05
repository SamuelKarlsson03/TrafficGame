using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCollider : MonoBehaviour
{
    [SerializeField] public int roadIndex;
    public void ChangeState(bool state)
    {
        RoadManager.instance.SetRoadStop(roadIndex, state);
    }
}
