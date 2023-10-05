using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heaven : MonoBehaviour
{
    [SerializeField] List<GameObject> scoreDisplays;

    [SerializeField] AudioClip godClip;

    AudioSource audioSource;

    bool soundPlayed = false;

    void Start()
    {
        Time.timeScale = 1f;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayGodSound());
    }

    private IEnumerator PlayGodSound()
    {
        yield return new WaitForSeconds(3f);
       audioSource.PlayOneShot(godClip);
       soundPlayed = true;
    }

    private void Update()
    {
        if (soundPlayed && !audioSource.isPlaying)
        {
            foreach (var item in scoreDisplays)
            {
                item.SetActive(true);
            }
        }
    }

}
