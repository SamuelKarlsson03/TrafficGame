using UnityEngine;
using Drawing;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SignControls : MonoBehaviour
{
    public XRDirectInteractor rHand;
    public XRDirectInteractor lHand;

    Vector3 direction;
    int rotationDirection = 1;

    Vector3 boxCastSize;
    readonly float maxReach = 5f;

    bool held = false;

    private void Start()
    {
        boxCastSize = GetComponent<BoxCollider>().size;
        direction = transform.TransformDirection(Vector3.right * rotationDirection);
    }

    void FixedUpdate()
    {
        if (held)
        {
            if (Physics.BoxCast(transform.position, boxCastSize / 2, direction, out RaycastHit hit, transform.rotation, maxReach))
            {
                if (hit.collider.CompareTag("Car"))
                {
                    Debug.Log("carhit");
                    //Do Stuff
                }
            }
        }
    }

    public void PickUpSign(GameObject hand)
    {
        if (this.CompareTag(rHand.interactablesSelected[0].transform.gameObject.tag))
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
        rotationDirection = (dotProduct >= 0) ? 1 : -1;

        direction = transform.TransformDirection(Vector3.right * rotationDirection);

        held = true;
    }

    public void DropSign()
    {
        held = false;
    }
}