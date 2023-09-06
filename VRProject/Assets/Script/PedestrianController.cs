using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    
    [SerializeField] bool moveInZAxis;
    [SerializeField] bool moveInXAxis;

    [SerializeField] bool keepMoving = true;

    Rigidbody rigidBody;


    private void Start()
    {
        
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (keepMoving)
        {
            Move();
        }
    }

    private void Move()
    {
        if (moveInXAxis)
        {
            rigidBody.velocity = new Vector3(moveSpeed, 0, 0) * Time.deltaTime;

        }

        if (moveInZAxis)
        {
            rigidBody.velocity = new Vector3(0, 0, moveSpeed) * Time.deltaTime;
        }

    }

    public void Stop()
    {
        rigidBody.velocity = Vector3.zero;
    }

    public void DecidePedestrianMovement(bool moveNegative, bool i_moveInXAxis)
    {

        if (moveNegative)
        {
            moveSpeed = moveSpeed * -1;
        }

        if (i_moveInXAxis)
        {
            moveInXAxis = true;
            moveInZAxis = false;
        }
        else
        {
            moveInXAxis = false;
            moveInZAxis = true;
        }
    }



}
