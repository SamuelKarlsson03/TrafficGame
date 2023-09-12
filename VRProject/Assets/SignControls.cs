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
        if (Physics.BoxCast(transform.position, boxCastSize / 2, Vector3.right, out RaycastHit hit, Quaternion.identity, maxReach))
        {
            Debug.Log(hit.collider.name);

            if (hit.collider.CompareTag("Car"))
                Debug.Log("carhit");
        }



        Draw.WireBox(transform.position + (Vector3.right*(maxReach / 2)), new Vector3(5f, boxCastSize.y, boxCastSize.z), Color.green);
    }
}