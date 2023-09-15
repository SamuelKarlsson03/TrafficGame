using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterManager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] GameObject rotationTarget;




    [Header("News caster Sounds")]
    [SerializeField] List<AudioClip> crashCastSounds;
    [SerializeField] List<AudioClip> ambulanceCastSounds;
    [SerializeField] AudioClip introductionSound;
    [SerializeField] bool hasIntroduced = false;
    [SerializeField] float timeUntilIntroduction = 3f;

    [Header("Helicopter Move Variables")]
    [SerializeField] float rotationAngle;
    [SerializeField] bool shouldMove = true;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayIntroduction());
    }

    private void Update()
    {
        if (shouldMove)
        {
            MoveHelicopter();
        }
    }

    public void PlayAudio(AudioClip clip, float volume = 1)
    {

        if (!audioSource.isPlaying && hasIntroduced)
        {
            audioSource.PlayOneShot(clip, volume);

        }

    }

    public void PlayHeliIntroductionsound(AudioClip audioClip, float volume = 1)
    {
        PlayAudio(audioClip, volume);
    }

    public void PlayRandomCrashCastSound(float volume = 1)
    {
       
        AudioClip clip = crashCastSounds[Random.Range(0, crashCastSounds.Count)];
        PlayAudio(clip, volume);
        
    }

    IEnumerator PlayIntroduction()
    {
        yield return new WaitForSeconds(timeUntilIntroduction);
        hasIntroduced = true;
        PlayAudio(introductionSound, 1f);
        yield return null;
    }

    private void MoveHelicopter()
    {
        transform.Rotate(rotationTarget.transform.position, rotationAngle * Time.deltaTime);

   
    }



}
