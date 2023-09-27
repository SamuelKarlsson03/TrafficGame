using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Throw, Return, Idle }
public class ThrowSign : MonoBehaviour
{
    public float signSpeed;
    Rigidbody rb;
    State state;

    public Transform playerHand;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Throw:
                {
                    rb.velocity = rb.velocity.normalized * signSpeed;
                    Debug.Log(rb.velocity);
                }

                break;
            case State.Return:
                {
                    Vector3 dir = playerHand.position - transform.position;
                    rb.velocity = dir * signSpeed;
                }

                break;
            case State.Idle:
                {
                    Debug.Log("Idle");
                }

                break;
            default:
                break;
        }
    }

    public void ThrowSigns()
    {
        state = State.Throw;
    }

    public void ReturnSigns()
    {
        state = State.Return;
    }

    public void IdleSigns()
    {
        state = State.Idle;
    }
}
