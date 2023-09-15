using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "intersection", menuName = "Road/Intersection")]
public class Intersection : ScriptableObject
{
    public Road intersectionRoad;
    public Road[] inboundRoads;
    public Road[] outgoingRoads;
}
