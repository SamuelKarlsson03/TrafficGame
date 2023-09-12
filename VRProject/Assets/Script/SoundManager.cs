using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<GameObject> loopingSoundObjs;

    GameObject newObj;
    [SerializeField] AudioClip[] audioClips;

    [Header("Sounds")]
    [SerializeField] List<AudioClip> engineSounds;
    [SerializeField] List<AudioClip> hornSounds;
    [SerializeField] List<AudioClip> screamSounds;
    [SerializeField] List<AudioClip> crashSounds;
    [SerializeField] List<AudioClip> tireSounds;
    [SerializeField] List<AudioClip> hitSounds;
    [SerializeField] List<AudioClip> alarmSounds;

    private void Awake() //Singleton instance 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        loopingSoundObjs = new List<GameObject>();


    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public GameObject PlayGlobalLoopingSound(AudioClip clip, float volume)
    {
        loopingSoundObjs.Add(CreateLoopAudioObject(clip, volume, transform.position, true));
        return loopingSoundObjs[loopingSoundObjs.Count - 1];
    }

    public GameObject PlayGlobalLoopingSound(AudioClip clip, float volume, float duration)
    {
        loopingSoundObjs.Add(CreateLoopAudioObject(clip, volume, transform.position, true));
        StartCoroutine(RemoveLoopingSoundAfterTime(clip, duration));
        return loopingSoundObjs[loopingSoundObjs.Count - 1];
    }

    public GameObject PlayLoopingAudioOnLocation(AudioClip clip, Vector3 location, float volume)
    {
        loopingSoundObjs.Add(CreateLoopAudioObject(clip, volume, location, false));
        return loopingSoundObjs[loopingSoundObjs.Count - 1];
    }

    public GameObject PlayLoopingAudioOnLocation(AudioClip clip, Vector3 location, float volume, float duration)
    {
        loopingSoundObjs.Add(CreateLoopAudioObject(clip, volume, location, false));
        StartCoroutine(RemoveLoopingSoundAfterTime(clip, duration));
        return loopingSoundObjs[loopingSoundObjs.Count - 1];
    }

    IEnumerator RemoveLoopingSoundAfterTime(AudioClip clip, float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveLoopingSound(clip);
    }

    public void RemoveLoopingSound(AudioClip clip)
    {
        foreach (var item in loopingSoundObjs)
        {
            if (item.GetComponent<AudioSource>().clip == clip)
            {

                loopingSoundObjs.Remove(item);
                Destroy(item);
                return;
            }
        }


    }

    private GameObject CreateLoopAudioObject(AudioClip clip, float volume, Vector3 position, bool isGlobal)
    {
        newObj = new GameObject("Name");
        newObj.name = "Looping sound obj: " + loopingSoundObjs.Count;
        newObj.transform.position = position;
        AudioSource source = newObj.AddComponent<AudioSource>();
        source.loop = true;
        source.spatialBlend = isGlobal ? 0 : 1;
        source.volume = volume;
        source.clip = clip;
        source.Play();
        return newObj;
    }

    public void PlayPitchedAudioOnLocation(AudioClip clip, Vector3 location, float volume = 1, float pitch = 1)
    {
        GameObject temp = new GameObject("AudioClip");
        temp.transform.position = location;
        AudioSource source = temp.AddComponent<AudioSource>();
        source.spatialBlend = 1;
        source.volume = volume;
        source.pitch = pitch;
        source.clip = clip;
        source.Play();
        Destroy(temp, clip.length);
    }

    public void PlayPitchedAudioOnLocation(AudioClip clip, Vector3 location, float volume = 1, float pitchMin = 0.9f, float pitchMax = 1.1f)
    {
        float pitch = Random.Range(pitchMin, pitchMax);
        PlayPitchedAudioOnLocation(clip, location, volume, pitch);
    }
    public void PlayAudioOnLocation(AudioClip clip, Vector3 location, float volume = 1) //Plays audioclip at specified location
    {

        AudioSource.PlayClipAtPoint(clip, location, volume);
    }


    public void PlayAudio(AudioClip clip, float volume = 1)
    {
        audioSource.PlayOneShot(clip, volume);

    }

    public void PlayRandomHornSound(Vector3 location, float volume = 1)
    {
        AudioClip clip = hornSounds[Random.Range(0, hornSounds.Count)];
        PlayAudioOnLocation(clip, location, volume);
    }

    public void PlayRandomScreamSound(Vector3 location, float volume = 1)
    {
        AudioClip clip = screamSounds[Random.Range(0, screamSounds.Count)];
        PlayAudioOnLocation(clip, location, volume);
    }

    public void PlayRandomHitSound(Vector3 location, float volume = 1)
    {
        AudioClip clip = hitSounds[Random.Range(0, hitSounds.Count)];
        PlayAudioOnLocation(clip, location, volume);
    }

    public void PlayRandomAlarmSoundLooping(Vector3 location, float volume = 1)
    {
        AudioClip clip = alarmSounds[Random.Range(0, alarmSounds.Count)];
        PlayLoopingAudioOnLocation(clip, location, volume);
    }

    public void PlayRandomTireSound(Vector3 location, float volume = 1)
    {
        AudioClip clip = tireSounds[Random.Range(0, tireSounds.Count)];
        PlayAudioOnLocation(clip, location, volume);
    }

    public void PlayRandomEngineSoundLooping(Vector3 location, float volume = 1)
    {
        AudioClip clip = engineSounds[Random.Range(0, engineSounds.Count)];
        PlayLoopingAudioOnLocation(clip, location, volume);
    }

    public void PlayRandomCrashSound(Vector3 location, float volume = 1)
    {
        AudioClip clip = crashSounds[Random.Range(0, crashSounds.Count)];
        PlayAudioOnLocation(clip, location, volume);
    }

}