using System.Collections;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    bool playing = false;
    // Method to play all Particle Systems in children
    public void PlayAllParticleSystemsInChildren()
    {
        if (playing) return;
        playing = true;
        StartCoroutine(EnableBlood());
        // Get all Particle Systems in children
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();

        // Play each Particle System
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }

    private IEnumerator EnableBlood()
    {
        yield return new WaitForSeconds(0.5f);
        playing = false;
    }
}
