using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] Road[] roads;
    [SerializeField] Intersection intersection;
    [SerializeField] float heightVisualization;
    public static RoadManager instance;

    [SerializeField] GameObject[] roadHighlight;

    [SerializeField] Color stopColor;
    [SerializeField] Color goColor;

    private List<Car>[] carsInRoads;
    private bool[] roadsStopping;

    private void Awake()
    {
        if (instance == null)
        {
            carsInRoads = new List<Car>[roads.Length];
            roadsStopping = new bool[roads.Length];
            for (int i = 0; i < roadsStopping.Length; i++)
            {
                roadsStopping[i] = i % 2 != 0;
                roadsStopping[i] = false;
            }
            for (int i = 0; i < carsInRoads.Length; i++)
            {
                carsInRoads[i] = new List<Car>();
            }

            for(int i = 0; i < roadsStopping.Length; i++)
            {
                SetRoadStop(i, roadsStopping[i]);
            }

            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private int GetIndexFromRoad(Road road)
    {
        for (int i = 0; i < roads.Length; i++)
        {
            if (road == roads[i])
            {
                return i;
            }
        }
        return -1;
    }

    public int AddCarToRoad(Car car, Road road)
    {
        int index = GetIndexFromRoad(road);
        carsInRoads[index].Add(car);
        return index;
    }

    public int AddCarToRoad(Car car, Vector3 point)
    {
        return AddCarToRoad(car, GetRoadFromPoint(point));
    }

    public bool GetStopFromRoad(int roadIndex)
    {
        return roadsStopping[roadIndex];
    }

    public void RemoveCarFromRoad(Car car, int index)
    {
        carsInRoads[index].Remove(car);
    }

    public void SetRoadStop(int roadIndex, bool stop)
    {
        if(roadIndex == 8)
        {
            return;
        }
        if (roadIndex == 0)
        {
            roadsStopping[roadIndex] = stop;
            for (int i = 0; i < carsInRoads[roadIndex].Count; i++)
            {
                carsInRoads[roadIndex][i].stopping = stop;
            }
        }
        else
        {
            roadsStopping[roadIndex+1] = stop;
            for (int i = 0; i < carsInRoads[roadIndex+1].Count; i++)
            {
                carsInRoads[roadIndex+1][i].stopping = stop;
            }
        }
        if (roadIndex < 8)
        {
            ParticleSystem[] particles = roadHighlight[roadIndex / 2].GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].startColor = stop ? stopColor : goColor;
            }
        }
    }

    public Car.Area GetAreaFromPoint(Vector3 point)
    {
        for (int i = 0; i < roads.Length; i++)
        {
            if (point.x > roads[i].startCorner.x &&
                point.x < roads[i].endCorner.x &&
                point.z > roads[i].startCorner.y &&
                point.z < roads[i].endCorner.y)
            {
                return roads[i].area;
            }
        }
        return Car.Area.offRoad;
    }

    public Road GetRoadFromPoint(Vector3 point)
    {
        for (int i = 0; i < roads.Length; i++)
        {
            if (point.x > roads[i].startCorner.x &&
                point.x < roads[i].endCorner.x &&
                point.z > roads[i].startCorner.y &&
                point.z < roads[i].endCorner.y)
            {
                return roads[i];
            }
        }
        return null;
    }

    public float DistanceToIntersection(Vector3 point)
    {
        Road currentRoad = GetRoadFromPoint(point);
        if (currentRoad == null) { return -1; }
        Intersection targetIntersection = currentRoad.intersection;
        Vector3 direction = currentRoad.roadDirection;
        if (currentRoad.area == Car.Area.towards)
        {
            if (Mathf.Abs(direction.x) > 0.5f)
            {
                return Mathf.Min(Mathf.Abs(point.x - targetIntersection.intersectionRoad.startCorner.x), Mathf.Abs(point.x - targetIntersection.intersectionRoad.endCorner.x));
            }
            else
            {
                return Mathf.Min(Mathf.Abs(point.z - targetIntersection.intersectionRoad.startCorner.y), Mathf.Abs(point.z - targetIntersection.intersectionRoad.endCorner.y));
            }
        }
        if (currentRoad.area == Car.Area.away || currentRoad.area == Car.Area.offRoad)
        {
            return 99999999f;
        }
        if (currentRoad.area == Car.Area.intersection)
        {
            return -1f;
        }
        return -1f;
    }

    public float DistanceFromCenterOfRoad(Vector3 point, Road road = null)
    {
        if (road == null)
        {
            road = GetRoadFromPoint(point);
        }
        Vector3 right = new Vector3(road.roadDirection.z, road.roadDirection.y, -road.roadDirection.x);
        if (Mathf.Abs(right.x) >= 0.95f)
        {
            return Mathf.Abs(point.x - ((road.startCorner.x + road.endCorner.x) * 0.5f));
        }
        if (Mathf.Abs(right.z) >= 0.95f)
        {
            return Mathf.Abs(point.z - ((road.startCorner.y + road.endCorner.y) * 0.5f));
        }
        return 5f;
    }

    public Road GetLeftTurnRoad(Road currentRoad)
    {
        for (int i = 0; i < roads.Length; i++)
        {
            if (currentRoad == roads[i])
            {
                return roads[(i + 5) % 8];
            }
        }
        return null;
    }

    public Road GetRightTurnRoad(Road currentRoad)
    {
        for (int i = 0; i < roads.Length; i++)
        {
            if (currentRoad == roads[i])
            {
                return roads[(i + 1) % 8];
            }
        }
        return null;
    }

    public Road GetRoadInFront(Road currentRoad)
    {
        for (int i = 0; i < roads.Length; i++)
        {
            if (currentRoad == roads[i])
            {
                return roads[((i + 3) + Mathf.FloorToInt((i + 3) / 8)) % 8];
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < roads.Length; i++)
        {
            Gizmos.color = Random.ColorHSV(0f, 1f, 0f, 1f, 0.75f, 1f);
            if (roads[i].color.r != 0 && roads[i].color.g != 0 && roads[i].color.b != 0)
            {
                Gizmos.color = roads[i].color;
            }
            else
            {
                roads[i].color = Gizmos.color;
            }
            Gizmos.DrawLine(new Vector3(roads[i].startCorner.x, heightVisualization, roads[i].startCorner.y), new Vector3(roads[i].startCorner.x, heightVisualization, roads[i].endCorner.y));
            Gizmos.DrawLine(new Vector3(roads[i].startCorner.x, heightVisualization, roads[i].startCorner.y), new Vector3(roads[i].endCorner.x, heightVisualization, roads[i].startCorner.y));
            Gizmos.DrawLine(new Vector3(roads[i].endCorner.x, heightVisualization, roads[i].startCorner.y), new Vector3(roads[i].endCorner.x, heightVisualization, roads[i].endCorner.y));
            Gizmos.DrawLine(new Vector3(roads[i].startCorner.x, heightVisualization, roads[i].endCorner.y), new Vector3(roads[i].endCorner.x, heightVisualization, roads[i].endCorner.y));
        }
    }
}
