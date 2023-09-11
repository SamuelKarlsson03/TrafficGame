using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CrosswalkEvent", menuName = "CrosswalkEvent")]
public class CrosswalkEvent : ScriptableObject
{
    public List<GameObject> pedestriansToSpawn;
    public bool spawnOnSameSide;
    public float timeBetweenSpawn;
    
}
