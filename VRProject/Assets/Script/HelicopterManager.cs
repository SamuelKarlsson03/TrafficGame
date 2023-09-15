using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterManager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] GameObject rotationTarget;

    [SerializeField] AudioClip introductionSound;
    [SerializeField] bool hasIntroduced = false;
    [SerializeField] float timeUntilIntroduction = 3f;

    [Header("Helicopter Move Variables")]
    [SerializeField] float rotationAngle;
    [SerializeField] bool shouldMove = true;
    [SerializeField] bool isMovingUpDown = false;
    [SerializeField] bool shouldMoveUp = true;
    [SerializeField] float horiMoveTime = 3f;
    [SerializeField] float horiMoveAmount = 0.5f;
    [SerializeField] float crashSpinAngle = 66f;
    [SerializeField] float heightReduction = 1f;

    [SerializeField] GameObject helicopterObject;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayIntroduction());
    }

    private void Update()
    {
        if (shouldMove)
        {
            RotateHelicopter();
        }

        if (!isMovingUpDown && shouldMove)
        {
           StartCoroutine(MoveHelicopterUpDown());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CrashHelicopter();
        }

        if (!shouldMove)
        {
            CrashMovement();
        }

    }

    public void PlayAudio(AudioClip clip, float volume = 1)
    {

        if (!audioSource.isPlaying && hasIntroduced)
        {
            audioSource.PlayOneShot(clip, volume);

        }

    }

    private void CrashMovement()
    {
        float randomHeightReduction = heightReduction * Random.Range(0.9f, 1.1f);
        transform.position += new Vector3(1,0,0);
       
        transform.Rotate(Vector3.up, crashSpinAngle * Time.deltaTime);
    }

    IEnumerator PlayIntroduction()
    {
        yield return new WaitForSeconds(timeUntilIntroduction);
        hasIntroduced = true;
        PlayAudio(introductionSound, 1f);
        yield return null;
    }

    private void RotateHelicopter()
    {
        transform.Rotate(rotationTarget.transform.position, rotationAngle * Time.deltaTime);
    }

    IEnumerator MoveHelicopterUpDown()
    {
        isMovingUpDown = true;
        shouldMoveUp = (!shouldMoveUp);

        float timeToMove = 0;

        float targetHeight = (horiMoveAmount + transform.position.y);

        if (shouldMoveUp)
        {

            while (timeToMove < horiMoveTime)
            {
                transform.position += new Vector3(0, horiMoveAmount / horiMoveTime * Time.deltaTime, 0);
                Debug.Log("Move Up");
                timeToMove += Time.deltaTime;
                yield return null;
            }
            //transform.position = new Vector3(0, targetHeight, 0);
        }

        if (!shouldMoveUp)
        {

            while (timeToMove < horiMoveTime)
            {
                horiMoveAmount *= -1;
                transform.position += new Vector3(0, horiMoveAmount / horiMoveTime * Time.deltaTime, 0);
                Debug.Log("Move Down");
                timeToMove += Time.deltaTime;
                yield return null;
            }
            //transform.position = new Vector3(0, targetHeight * -1, 0);
        }

        isMovingUpDown = false;
        yield return null;

    }

    private void CrashHelicopter()
    {
        shouldMove = false;
        StopAllCoroutines();

        Vector3 newPos = helicopterObject.transform.position;
        transform.position = newPos;
        helicopterObject.transform.localPosition = Vector3.zero;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Controller"))
        {
            SoundManager.Instance.PlayRandomHelicopterHitByControllerSound(1f);
        }

        if (collision.gameObject.CompareTag("Car"))
        {
            SoundManager.Instance.PlayRandomHelicopterGoingDownSound(1f);
            CrashHelicopter();
        }
    }

    

}
