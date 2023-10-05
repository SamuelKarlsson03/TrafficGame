using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nuke : MonoBehaviour
{
    [SerializeField] AudioClip nukeAfterSound;
    [SerializeField] AudioClip nukeExplosion;
    [SerializeField] AudioClip barackObamaAnnounce;

    [SerializeField] GameObject blast;
    [SerializeField] GameObject nukeVisualObject;

    [SerializeField] bool hasDropped = false;
    [SerializeField] float dropSpeed = 10f;
    [SerializeField] float explosionPower = 100f;
    [SerializeField] bool hasExploded = false;

    AudioSource audioSource;

    float timer;
    [SerializeField] float timeUntilNuke = 90f;
    bool hasAnnouncedNuke = false;

    void Start()
    {
        blast.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeUntilNuke && !hasAnnouncedNuke)
        {
            audioSource.PlayOneShot(barackObamaAnnounce);
            hasAnnouncedNuke = true;
            SoundManager.Instance.audioSource.Stop();
            SoundManager.Instance.RemoveSong();
            HelicopterManager.Instance.audioSource.Stop();
            SoundManager.Instance.audioSource.enabled = false;
            HelicopterManager.Instance.audioSource.enabled = false;
        }

        if (hasAnnouncedNuke && !audioSource.isPlaying)
        {
            DropNuke();
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            DropNuke();
        }

        if (hasDropped && !hasExploded)
        {
            transform.position -= new Vector3(0, 1 * dropSpeed, 0) * Time.deltaTime;
        }

        if (hasExploded)
        {
            blast.transform.localScale += new Vector3(explosionPower, explosionPower, explosionPower) * Time.deltaTime;
        }

        if (transform.position.y <= 0.5f)
        {
            if (!hasExploded)
            {
                hasExploded = true;
                StartCoroutine(ExplodeNuke());

            }
        }

        if (blast.transform.localScale.x > 1.55)
        {
            SceneManager.LoadScene("Heaven");
        }

    }

    private IEnumerator ExplodeNuke()
    {
        nukeVisualObject.GetComponent<MeshRenderer>().enabled = false;
        blast.SetActive(true);
        audioSource.PlayOneShot(nukeAfterSound, 0.25f);
        yield return new WaitForSeconds(10f);
        Time.timeScale = 1f;
    }

    public void DropNuke()
    {
        
        SoundManager.Instance.audioSource.pitch = 0.5f;
        HelicopterManager.Instance.audioSource.pitch = 0.5f;
        Time.timeScale = 0.1f;
        hasDropped = true;
    }

  
  

  

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        SceneManager.LoadScene("Heaven");
    //    }
    //}


}
