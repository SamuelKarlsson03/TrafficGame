using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gramophoneTutManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] GameObject explosionObject;
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] GameObject explosion;
    private int currentAudioClipIndex = 0;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayNextClip()
    {
        if (currentAudioClipIndex < audioClips.Count)
        {
            audioSource.clip = audioClips[currentAudioClipIndex];
            audioSource.Play();
            currentAudioClipIndex++;
        }
        else
        {
            DestroyAndExplode();
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying && currentAudioClipIndex <= audioClips.Count)
        {
            PlayNextClip();
        }
    }

    private void DestroyAndExplode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        explosionObject.SetActive(true);
        Destroy(gameObject);
    }

}
