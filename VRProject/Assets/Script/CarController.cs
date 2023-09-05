using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public Transform explosionSpawn;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Car"))
        {
            Destroy(gameObject);

            Instantiate(explosionPrefab, explosionSpawn.position, explosionSpawn.rotation);
            Destroy(explosionPrefab, 2);
        }
    }
}
