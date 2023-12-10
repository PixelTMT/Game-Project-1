using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    AudioSFX audioSFX;
    private void Awake()
    {
        audioSFX = GameObject.FindFirstObjectByType<AudioSFX>();
    }
    public void Play_Sound(string name)
    {
        audioSFX.Play_Sound(name);
    }
    public void StopSound(string name)
    {
        audioSFX.StopSound(name);
    }
}
