using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5;

     bool shouldMoveByItself = false;
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

        if (shouldMoveByItself)
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

    public void StartMovement(Transform endPos)
    {

        moveDirection = (endPos.position - transform.position).normalized;

        shouldMoveByItself = true;
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
        transform.localPosition += moveToSideDist * transform.right;
    }

    public void CheckIfDie()
    {
        if (hasBeenHit)
        {
            Die();
        }

    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void HitByVehicle()
    {
        hasBeenHit = true;
        rigidBody.constraints = RigidbodyConstraints.None;
        shouldMoveByItself = false;
        Debug.Log("Hit by Vehicle");
    }


}
