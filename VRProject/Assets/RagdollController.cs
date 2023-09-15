using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private Rigidbody[] rigidbodies; // Store the rigidbodies of the ragdoll parts
    private Animator animator; // Reference to the Animator

    public float collisionVelocityThreshold = 5f; // Minimum X velocity to trigger ragdoll

    private void Start()
    {
        // Get references to the rigidbodies and animator
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        // Disable the ragdoll initially
        SetRagdollEnabled(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("ow");
        if (collision.transform.root == this.transform || collision.CompareTag("Ground"))
            return;

        // Calculate the magnitude of the collision velocity vector
        float collisionVelocityMagnitude = collision.attachedRigidbody.velocity.magnitude;

        // If the collision's velocity magnitude is greater than the threshold, activate ragdoll
        if (collisionVelocityMagnitude >= collisionVelocityThreshold)
        {
            SetRagdollEnabled(true);

            // Inherit the velocity from what hit the ragdoll parts
            Vector3 collisionVelocity = collision.attachedRigidbody.velocity;
            foreach (Rigidbody rb in rigidbodies)
            {
                // Apply the collision velocity to each ragdoll part
                rb.velocity = collisionVelocity;
            }
        }
    }

    private void SetRagdollEnabled(bool isEnabled)
    {
        // Enable or disable the rigidbodies and animator
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !isEnabled;
        }

        animator.enabled = !isEnabled;
    }

    private bool IsRagdollEnabled()
    {
        // Check if any of the ragdoll's rigidbodies is not kinematic
        foreach (Rigidbody rb in rigidbodies)
        {
            if (!rb.isKinematic)
            {
                return true;
            }
        }
        return false;
    }
}
