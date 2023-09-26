using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float volume;
    [SerializeField] List<AudioClip> explosionSounds;
    void Start()
    {
        SoundManager.Instance.PlayAudioOnLocation(explosionSounds[Random.Range(0, explosionSounds.Count)], transform.position, volume);
        Destroy(this.gameObject, 2f);
    }
}
