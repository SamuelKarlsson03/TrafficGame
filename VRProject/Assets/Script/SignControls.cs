using UnityEngine;
using Drawing;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using System.Collections;

public class SignControls : MonoBehaviour
{
    public XRDirectInteractor rHand;
    public XRDirectInteractor lHand;

    Vector3 direction;
    float rotationDirection = 1;

    Vector3 boxCastSize;
    [SerializeField] float maxReach = 100f;
    [SerializeField] float despawnTime = 3f;

    public bool held = false;
    [SerializeField] bool stopSign;

    private void Start()
    {
        boxCastSize = GetComponent<BoxCollider>().size * 50;
        direction = transform.right * rotationDirection;
    }

    void FixedUpdate()
    {
        direction = transform.right * rotationDirection;
        Debug.Log("HELD = "+held);
        if (held)
        {
            //Draw.WireBox(transform.TransformPoint(Vector3.right * (maxReach / 2) * rotationDirection), transform.rotation, new Vector3(maxReach, boxCastSize.y, boxCastSize.z), Color.green);

            RaycastHit[] hits = Physics.BoxCastAll(transform.position, boxCastSize / 2, direction, Quaternion.identity, maxReach,LayerMask.GetMask("Car"));
            Debug.Log("CARSINRANGE"+hits.Length);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Car"))
                {
                    Debug.Log("carhit");
                    hits[i].collider.GetComponent<Car>().stopping = stopSign;
                }
            }
        }
     }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (direction*maxReach));
        //Gizmos.DrawCube(transform.position + direction * (maxReach / 2), boxCastSize + direction*maxReach);
    }

    public void PickUpSign()
    {
        GameObject hand = null;

        if (rHand.interactablesSelected.Count > 0 && this.CompareTag(rHand.interactablesSelected[0].transform.gameObject.tag))
            hand = rHand.gameObject;
        else if (this.CompareTag(lHand.interactablesSelected[0].transform.gameObject.tag))
            hand = lHand.gameObject;

        // Get the forward direction of the hand (Z-axis).
        Vector3 handForward = hand.transform.forward;

        // Get the correct forward direction of the stop sign (X-axis).
        Vector3 stopSignForward = transform.right; // Assuming stop sign's forward is along the X-axis.

        // Calculate the dot product between the hand's forward direction and stop sign's forward direction.
        float dotProduct = Vector3.Dot(handForward, stopSignForward);

        // Determine the sign (1 or -1) based on the dot product.
        rotationDirection = (dotProduct >= 0) ? 1f : -1f;

        direction = transform.right * rotationDirection;

        held = true;
        CancelInvoke(nameof(Despawn));
    }

    public void DropSign()
    {
        if (rHand.interactablesSelected.Count > 0 && this.CompareTag(rHand.interactablesSelected[0].transform.gameObject.tag))
            return;
        else if (this.CompareTag(lHand.interactablesSelected[0].transform.gameObject.tag))
            return;

        held = false;
        Invoke(nameof(Despawn), despawnTime);
    }

    private void Despawn()
    {
        if(!held)
        {
            Destroy(gameObject);
        }
    }
}