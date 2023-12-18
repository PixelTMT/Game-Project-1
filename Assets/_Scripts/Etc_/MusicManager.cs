using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] audioClips;

    AudioSource audio;
    private IEnumerator Start()
    {
        audio = GetComponent<AudioSource>();
        while (true)
        {
            yield return new WaitWhile(()=> audio.isPlaying);
            audio.clip = audioClips[Random.Range(0, audioClips.Length)];
            audio.Play();
        }
    }
}
