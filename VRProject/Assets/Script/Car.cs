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
    private Road currentRoad;
    private bool hasTurned;

    public bool stopping;
    private bool shouldStop;
    private bool grounded;
    private Vector3 velocity;
    private int turnDirection;
    Car carInFront;

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float health;
    public bool broken;
    [SerializeField] float crashSoundCooldown = 1f;
    float timer;
    bool canMakeCrashSound = true;

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
        turnDirection = 1;
        shouldStop = Random.Range(0, 2) == 0;
        shouldStop = false;
        currentRoad = RoadManager.instance.GetRoadFromPoint(transform.position);
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
        timer += Time.deltaTime;

        if (timer >= crashSoundCooldown)
        {
            canMakeCrashSound = true;
            timer -= crashSoundCooldown;
        }

        if (RoadManager.instance.GetAreaFromPoint(transform.position) == Area.away)
        {
            shouldStop = false;
        }
        else
        {
            shouldStop = stopping;
        }

        if(broken)
        {
            shouldStop = true;
            stopping = true;
        }
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

        if(broken)
        {
            return;
        }

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

        if (!hasTurned)
        {
            if (RoadManager.instance.GetAreaFromPoint(transform.position) == Area.intersection)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        currentRoad = RoadManager.instance.GetLeftTurnRoad(currentRoad);
                        Debug.Log(gameObject.name + ": turning left");
                        break;
                    case 1:
                        currentRoad = RoadManager.instance.GetRightTurnRoad(currentRoad);
                        Debug.Log(gameObject.name + ": turning right");
                        break;
                    default:
                        currentRoad = RoadManager.instance.GetRoadInFront(currentRoad);
                        Debug.Log(gameObject.name + ": going forward");
                        break;
                }
                //Debug.Log(gameObject.name + " is turning from " + currentRoad.name);
                //currentRoad = RoadManager.instance.GetLeftTurnRoad(currentRoad);
                //Debug.Log(gameObject.name + " is turning to" + currentRoad.name);
                //Debug.Log(gameObject.name + ": turning left");
                hasTurned = true;
            }
        }

        //Stablilize to lane

        Vector3 positionInFuture = frontPosition.position + velocity * futureLookTime;
        Road roadInFuture = currentRoad;
        if (roadInFuture != null)
        {
            if (roadInFuture.roadDirection.magnitude > 0.001f)
            {
                if (Vector3.Dot(roadInFuture.roadDirection, transform.forward) < 0.76f)
                {
                    transform.Rotate(Vector3.up, turnDirection * turnSpeed * Time.deltaTime);
                    Vector3 forwardLeft = transform.forward;
                    transform.Rotate(Vector3.up, -2 * turnDirection * turnSpeed * Time.deltaTime);
                    Vector3 forwardRight = transform.forward;
                    if (Vector3.Dot(roadInFuture.roadDirection, forwardLeft) > Vector3.Dot(roadInFuture.roadDirection, forwardRight))
                    {
                        transform.Rotate(Vector3.up, 2 * turnDirection * turnSpeed * Time.deltaTime);
                    }
                    else
                    {
                    }
                }
                else if (RoadManager.instance.DistanceFromCenterOfRoad(positionInFuture, roadInFuture) > 0.75f)
                {
                    Vector3 positionInFutureLeft = frontPosition.position + (velocity + transform.right * Time.deltaTime) * futureLookTime;
                    Vector3 positionInFutureRight = frontPosition.position + (velocity + transform.right * -Time.deltaTime) * futureLookTime;
                    if (RoadManager.instance.DistanceFromCenterOfRoad(positionInFutureLeft, roadInFuture) > RoadManager.instance.DistanceFromCenterOfRoad(positionInFutureRight, roadInFuture))
                    {
                        transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
                    }
                    else
                    {
                        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
                    }

                }
                else if (Vector3.Dot(roadInFuture.roadDirection, transform.forward) < 0.9999f)
                {
                    transform.Rotate(Vector3.up, turnDirection * turnSpeed * Time.deltaTime);
                    Vector3 forwardLeft = transform.forward;
                    transform.Rotate(Vector3.up, -2 * turnDirection * turnSpeed * Time.deltaTime);
                    Vector3 forwardRight = transform.forward;
                    if (Vector3.Dot(roadInFuture.roadDirection, forwardLeft) > Vector3.Dot(roadInFuture.roadDirection, forwardRight))
                    {
                        transform.Rotate(Vector3.up, 2 * turnDirection * turnSpeed * Time.deltaTime);
                    }
                    else
                    {
                        transform.Rotate(Vector3.up, turnDirection * turnSpeed * Time.deltaTime);
                    }
                }
            }
        }

        if (shouldStop)
        {
            if (distanceIfNoVelocityChange <= 0)
            {
                //Slow down
                SlowDown();
                return;
            }
        }
        if (shouldStop)
        {

            float distanceToIntersection = RoadManager.instance.DistanceToIntersection(transform.position);
            if (distanceToIntersection != -1 && distanceToIntersection < 3f + velocity.magnitude * desiredTimeDistanceToFront)
            {
                SlowDown();
                return;
            }
            else
            {
                //Turn right/left or continue going straight
            }
        }

        if (shouldStop && velocity.magnitude - 2f > speedLimit)
        {
            SlowDown();
            return;
        }

        if (shouldStop)
        {
            if (distanceIfNoVelocityChange >= acceptableStoppingDistance - (velocity - carInFrontVelocity).magnitude * desiredTimeDistanceToFront && velocity.magnitude < speedLimit + 3f)
            {
                SpeedUp();
                return;
            }
        }
        else
        {
            if (velocity.magnitude < speedLimit + 3f)
            {
                SpeedUp();
                return;
            }
        }

        if (shouldStop && velocity.magnitude < speedLimit && RoadManager.instance.GetAreaFromPoint(frontPosition.position + velocity * 2f) == Area.offRoad || RoadManager.instance.GetAreaFromPoint(frontPosition.position + velocity * 1f) == Area.intersection)
        {
            SlowDown();
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

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Car") && canMakeCrashSound)
        {

            float velocityForce = (velocity - collision.relativeVelocity).magnitude;

            canMakeCrashSound = false;
            health -= velocityForce;
            if(health < 0)
            {
                Explode((transform.position + collision.gameObject.transform.position) * 0.5f);
            }
            SoundManager.Instance.PlayRandomCrashCastSound();
            SoundManager.Instance.PlayRandomCrashSound(transform.position, Mathf.Lerp(0f, 1f, velocityForce / 100f));

        }
    }

    private void Explode(Vector3 point)
    {
        Instantiate(explosionPrefab).transform.position = point + Random.insideUnitSphere.normalized + new Vector3(0,1f,0);
        broken = true;
        Destroy(this.gameObject, 5f);
    }

}
