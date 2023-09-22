using UnityEngine;
using Drawing;

public class SignControls : MonoBehaviour
{
    [SerializeField] Vector3 boxCastSize;
    [SerializeField] bool stopSign;
    [SerializeField] float maxReach = 5f;

    private void Start()
    {
        //boxCastSize = GetComponent<BoxCollider>().size;
    }

    void FixedUpdate()
    {
        Vector3 direction = transform.TransformDirection(Vector3.right);
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, boxCastSize / 2, direction, out hit, transform.rotation, maxReach) 
            || Physics.BoxCast(transform.position, boxCastSize / 2, -direction, out hit, transform.rotation, maxReach))
        {
            if (hit.collider.CompareTag("Car"))
            {
                Debug.Log("carhit");
                hit.collider.GetComponent<Car>().stopping = stopSign;
            }
        }

        //Debug
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.TransformPoint(Vector3.right * (maxReach / 2)), new Vector3(maxReach, boxCastSize.y, boxCastSize.z));
    }
}