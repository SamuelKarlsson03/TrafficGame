using UnityEngine;
using Drawing;

public class SignControls : MonoBehaviour
{
    Vector3 boxCastSize;
    float maxReach = 5f;

    private void Start()
    {
        boxCastSize = GetComponent<BoxCollider>().size;
    }

    void Update()
    {
        Vector3 direction = transform.TransformDirection(Vector3.right);
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, boxCastSize / 2, direction, out hit, transform.rotation, maxReach) 
            || Physics.BoxCast(transform.position, boxCastSize / 2, -direction, out hit, transform.rotation, maxReach))
        {
            if (hit.collider.CompareTag("Car"))
            {
                Debug.Log("carhit");
                //Do Stuff
            }
        }

        //Debug
        Draw.WireBox(transform.TransformPoint(Vector3.right * (maxReach / 2)), transform.rotation, new Vector3(5f, boxCastSize.y, boxCastSize.z), Color.green);
    }
}