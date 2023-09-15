using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Road", menuName ="Road/Road")]
public class Road : ScriptableObject
{
    public Color color;
    public Vector2 startCorner;
    public Vector2 endCorner;
    public Intersection intersection;
    public Car.Area area;
    public Vector3 roadDirection;
}
