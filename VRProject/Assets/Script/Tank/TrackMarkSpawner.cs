using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMarkSpawner : MonoBehaviour
{
    public GameObject trackPrefab;
    private Rigidbody rb;

    [Header("Timers")]
    public float spawnTime;
    public float Timer;
    public float deSpawnTrack;

    void Start()
    {
        Timer = spawnTime;

        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        Debug.Log(Timer);

        if (Timer <= 0)
        {
            if (rb != null)
            {
                if (rb.velocity.magnitude > 0.01f)
                {
                    Quaternion myRotation = Quaternion.Euler(90f, 0f, 0f);
                    GameObject newTrack = Instantiate(trackPrefab, transform.position, myRotation);

                    Destroy(newTrack, deSpawnTrack);
                }
            }

            Timer = spawnTime;
        }
    }
}
