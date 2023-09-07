using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    [SerializeField] bool shouldMove = false;
    [SerializeField] bool shouldCheckForPedestrians = true;
    [SerializeField] bool hasBeenHit = false;

    Rigidbody rigidBody;

    [SerializeField] float raycastDistance = 1f;
    [SerializeField] float moveToSideDist = 1f;

    Vector3 moveDirection;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (shouldMove)
        {
            Move();
        }

        if (shouldCheckForPedestrians)
        {
            CheckForOtherPedestrians();
        }

    }

    private void Move()
    {

        rigidBody.velocity = moveDirection * moveSpeed;
    }

    public void StartMovement(Transform startPos, Transform endPos)
    {

        moveDirection = (endPos.position - transform.position).normalized;

        shouldMove = true;


    }

    public void CheckForOtherPedestrians()
    {

        Vector3 raycastOrigin = transform.position;

        Vector3 raycastDirection = transform.forward;

        Ray ray = new Ray(raycastOrigin, raycastDirection);

        RaycastHit hit;

        GetComponent<Collider>().enabled = false;

        Debug.DrawRay(raycastOrigin, raycastDirection, Color.yellow);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Pedestrian"))
            {
                Debug.Log("Pedestrian Hit");
                MoveToTheSide();
            }
        }

        GetComponent<Collider>().enabled = true;

    }

    private void MoveToTheSide()
    {
        Debug.Log("Should move to the side");
        transform.position += new Vector3(0, 0, moveToSideDist);
    }

    public void CheckIfDie()
    {
        if (hasBeenHit)
        {
            Die();
        }

    }

    private void Die()
    {
        Debug.Log("Man im dead PLACE OF DEATH : " + transform.position.z);
        Destroy(gameObject);
    }

    public void HitByVehicle()
    {
        hasBeenHit = true;
        rigidBody.constraints = RigidbodyConstraints.None;
        shouldMove = false;
        Debug.Log("Hit by Vehicle");
    }


}
