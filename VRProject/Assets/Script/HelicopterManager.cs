using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterManager : MonoBehaviour
{
    public static HelicopterManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [SerializeField] GameObject rotaryObject;
    [SerializeField] float rotateSpeed = 500f;

    public AudioSource audioSource;
    Animator animator;

    [SerializeField] AudioClip introductionSound;
    [SerializeField] bool hasIntroduced = false;
    [SerializeField] float timeUntilIntroduction = 3f;

    [SerializeField] public bool isCurrentlyPlayingAudio;

    [Header("Helicopter Move Variables")]
    [SerializeField] bool hasCrashed = false;
    [SerializeField] float crashSpinAngle = 66f;
    [SerializeField] float fallForce;

    float castCooldown = 3f;
    float timer;
    public bool canCast = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayIntroduction());
    }

    private void Update()
    {
        

        if (!audioSource.isPlaying)
        {
            timer += Time.deltaTime;
        }

        if (timer >= castCooldown)
        {
            timer = 0;
            canCast = true;
        }

        if (audioSource.isPlaying)
        {
            isCurrentlyPlayingAudio = true;
        }
        else
        {
            isCurrentlyPlayingAudio = false;
        }


        if (hasCrashed)
        {
            CrashMovement();
        }

        RotateBlades();

    }

    private void RotateBlades()
    {
        rotaryObject.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void StopAllAudioCurrentyPlaying()
    {
        audioSource.Stop();
    }

    public void PlayAudio(AudioClip clip, float volume = 1)
    {
        if (!audioSource.isPlaying && hasIntroduced && canCast && !hasCrashed)
        {
            canCast = false;
            audioSource.PlayOneShot(clip, volume);

        }

    }

    private void CrashMovement()
    {
        transform.localPosition += new Vector3(10, -fallForce, 0) * Time.deltaTime;

        transform.Rotate(0, crashSpinAngle * Time.deltaTime, 0);
    }

    IEnumerator PlayIntroduction()
    {
        yield return new WaitForSeconds(timeUntilIntroduction);
        hasIntroduced = true;
        PlayAudio(introductionSound, 1f);
        yield return null;
    }

    private void CrashHelicopter()
    {
        hasCrashed = true;
        StopAllCoroutines();
        animator.enabled = false;
        SoundManager.Instance.PlayRandomHelicopterGoingDownSound(1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Controller"))
        {
            SoundManager.Instance.PlayRandomHelicopterHitByControllerSound(1f);
        }

        if (collision.gameObject.CompareTag("Car"))
        {
            CrashHelicopter();
        }
    }

}
