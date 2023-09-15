using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] float inspectCarInfrontTimer;
    private float currentInspectCarInfrontTimer;

    [SerializeField] float desiredTimeDistanceToFront;
    [SerializeField] float breakStrength;
    [SerializeField] float breakForce;
    [SerializeField] float acceleration;
    [SerializeField] float maxPossibleSpeed;
    [SerializeField] float speedLimit;
    [SerializeField] float acceptableStoppingDistance;
    [SerializeField] float turnSpeed;
    [SerializeField] float speedWhileTurning;
    [SerializeField] float friction;
    [SerializeField] float futureLookTime;

    [SerializeField] Transform frontPosition;
    [SerializeField] Transform backPosition;

    public bool shouldStop;
    private bool grounded;
    private Vector3 velocity;
    private int turnDirection;
    Car carInFront;

    public enum Area
    {
        towards,
        away,
        intersection,
        offRoad
    }

    // Start is called before the first frame update
    void Start()
    {
        turnDirection = Random.Range(-1, 2);
        shouldStop = true;
        rb = GetComponent<Rigidbody>();
    }

    [ContextMenu("Create front and back")]
    public void CreateFrontBack()
    {
        frontPosition = new GameObject("Front").transform;
        frontPosition.parent = transform;
        frontPosition.position = Physics.ClosestPoint(transform.position + transform.forward * 10, GetComponent<Collider>(), transform.position, Quaternion.identity);
        backPosition = new GameObject("Back").transform;
        backPosition.parent = transform;
        backPosition.position = Physics.ClosestPoint(transform.position + transform.forward * -10, GetComponent<Collider>(), transform.position, Quaternion.identity);
    }

    void Update()
    {
        grounded = Grounded();
        if (grounded)
        {
            rb.MovePosition(transform.position + (velocity * Time.deltaTime));
            UpdateVelocity();
            rb.velocity = velocity;
        }
        else
        {
            velocity = rb.velocity;
        }
        currentInspectCarInfrontTimer -= Time.deltaTime;
        if (currentInspectCarInfrontTimer <= 0)
        {
            currentInspectCarInfrontTimer += inspectCarInfrontTimer;
            carInFront = GetCarInFront();
        }
    }

    private void UpdateVelocity()
    {
        velocity /= Mathf.Pow(friction, Time.deltaTime);
        float distanceIfNoVelocityChange;
        Vector3 direction = transform.forward;
        direction.y = 0;
        direction.Normalize();
        Vector3 carInFrontVelocity = Vector3.zero;
        if (carInFront == null)
        {
            distanceIfNoVelocityChange = 999;
        }
        else
        {
            Vector3 frontCarVelocity = carInFront.GetVelocity();
            carInFrontVelocity = frontCarVelocity;
            distanceIfNoVelocityChange = Vector3.Distance((frontPosition.position + velocity * desiredTimeDistanceToFront), (carInFront.backPosition.position + frontCarVelocity * desiredTimeDistanceToFront));
            if (Vector3.Dot(((frontPosition.position + velocity * desiredTimeDistanceToFront) - (carInFront.backPosition.position + frontCarVelocity * desiredTimeDistanceToFront)).normalized, direction) > 0)
            {
                distanceIfNoVelocityChange *= -1f;
            }
        }

        Road currentRoad = RoadManager.instance.GetRoadFromPoint(transform.position);

        //Stablilize to lane

        //Vector3 positionInFuture = frontPosition.position + velocity * futureLookTime;
        //Road roadInFuture = RoadManager.instance.GetRoadFromPoint(positionInFuture);
        //if (roadInFuture != null)
        //{
        //    if (roadInFuture.roadDirection.magnitude > 0.001f)
        //    {
        //        if (Vector3.Dot(roadInFuture.roadDirection, transform.forward) < 0.95)
        //        {
        //            transform.Rotate(Vector3.up, turnDirection * turnSpeed * Time.deltaTime);
        //            Vector3 forwardLeft = transform.forward;
        //            transform.Rotate(Vector3.up, -2 * turnDirection * turnSpeed * Time.deltaTime);
        //            Vector3 forwardRight = transform.forward;
        //            if (Vector3.Dot(roadInFuture.roadDirection, forwardLeft) > Vector3.Dot(roadInFuture.roadDirection, forwardRight))
        //            {
        //                transform.Rotate(Vector3.up, 2 * turnDirection * turnSpeed * Time.deltaTime);
        //            }
        //        }
        //        else if (RoadManager.instance.DistanceFromCenterOfRoad(positionInFuture, roadInFuture) > 1.5f)
        //        {
        //            Vector3 positionInFutureLeft = frontPosition.position + (velocity + transform.right * Time.deltaTime) * futureLookTime;
        //            Vector3 positionInFutureRight = frontPosition.position + (velocity + transform.right * -Time.deltaTime) * futureLookTime;
        //            if (RoadManager.instance.DistanceFromCenterOfRoad(positionInFutureLeft, roadInFuture) > RoadManager.instance.DistanceFromCenterOfRoad(positionInFutureRight, roadInFuture))
        //            {
        //                transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        //            }
        //            else
        //            {
        //                transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        //            }

        //        }
        //    }
        //}

        if (distanceIfNoVelocityChange <= 0)
        {
            //Slow down
            SlowDown();
            return;
        }

        float distanceToIntersection = RoadManager.instance.DistanceToIntersection(transform.position);
        if (distanceToIntersection != -1 && distanceToIntersection < 3f + velocity.magnitude * desiredTimeDistanceToFront)
        {
            if (shouldStop)
            {
                SlowDown();
                return;
            }
            else
            {
                //Turn right/left or continue going straight
            }
        }

        if (velocity.magnitude - 2f > speedLimit)
        {
            SlowDown();
            return;
        }

        if (distanceIfNoVelocityChange >= acceptableStoppingDistance - (velocity - carInFrontVelocity).magnitude * desiredTimeDistanceToFront && velocity.magnitude < speedLimit + 3f)
        {
            SpeedUp();

            return;
        }

        if (velocity.magnitude < speedLimit && RoadManager.instance.GetAreaFromPoint(frontPosition.position + velocity * 2f) == Area.offRoad || RoadManager.instance.GetAreaFromPoint(frontPosition.position + velocity * 2f) == Area.intersection)
        {
            //Chagne direction slightly
            SlowDown();
            if (velocity.magnitude < speedLimit && RoadManager.instance.GetAreaFromPoint(frontPosition.position + velocity * 2f) == Area.offRoad || RoadManager.instance.GetAreaFromPoint(frontPosition.position + velocity * 2f) == Area.intersection)
            {
                transform.Rotate(Vector3.up, turnSpeed * turnDirection * Time.deltaTime);
            }
            return;
        }
        else
        {
            //Keep velocity
        }

    }

    private void SlowDown()
    {
        velocity /= Mathf.Pow(breakStrength * 0.5f, Time.deltaTime);
        float force = velocity.sqrMagnitude;
        float newForce = Mathf.Max(force - Time.deltaTime * breakForce, 0);
        velocity = velocity.normalized * Mathf.Sqrt(newForce);
        velocity /= Mathf.Pow(breakStrength * 0.5f, Time.deltaTime);
    }

    private void SpeedUp()
    {
        Vector3 direction = transform.forward;
        direction.y = 0;
        direction.Normalize();
        float speedOutOfMaxPossible = 1f - (velocity.magnitude / maxPossibleSpeed);
        velocity += direction * acceleration * Time.deltaTime * speedOutOfMaxPossible;
    }

    public Car GetCarInFront()
    {
        RaycastHit hit;
        GetComponent<Collider>().enabled = false;
        if (Physics.SphereCast(frontPosition.position - transform.forward * 2.5f, 2.5f, transform.forward, out hit, 100, LayerMask.GetMask("Car"), QueryTriggerInteraction.UseGlobal))
        {
            GetComponent<Collider>().enabled = true;
            Car car;
            if (hit.transform.TryGetComponent<Car>(out car))
            {
                carInFront = car;
            }
            return car;
        }
        GetComponent<Collider>().enabled = true;
        return null;
    }

    private bool Grounded()
    {
        if (Physics.OverlapSphere(transform.position - transform.up * 0.5f, 1f).Length > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }
}
