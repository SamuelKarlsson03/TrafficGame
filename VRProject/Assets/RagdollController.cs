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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ow");

        // Check the collision's relative velocity in the X-axis
        float collisionVelocityX = Mathf.Abs(collision.relativeVelocity.x);

        // If the collision's X velocity is greater than the threshold, activate ragdoll
        if (collisionVelocityX >= collisionVelocityThreshold)
        {
            SetRagdollEnabled(true);
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
