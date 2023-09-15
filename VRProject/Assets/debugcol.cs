using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugcol : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public Axis axis = Axis.X; // SerializeField to select the axis
    public float velocity = 5f; // SerializeField for the velocity

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireWithVelocity();
        }
    }

    private void FireWithVelocity()
    {
        Vector3 force = Vector3.zero;

        // Determine the axis based on the selected enum
        switch (axis)
        {
            case Axis.X:
                force = Vector3.right * velocity;
                break;
            case Axis.Y:
                force = Vector3.up * velocity;
                break;
            case Axis.Z:
                force = Vector3.forward * velocity;
                break;
        }

        // Apply the velocity as an impulse
        rb.AddForce(force, ForceMode.Impulse);
    }
}
