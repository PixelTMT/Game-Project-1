using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFX : MonoBehaviour
{
    Dictionary<string, AudioSource> sfx = new Dictionary<string, AudioSource>();
    Transform _SoundSFX;
    private void Awake()
    {
        _SoundSFX = transform;
    }
    public void Play_Sound(string name)
    {
        try
        {
            if (!sfx.ContainsKey(name))
                sfx.Add(name, _SoundSFX.Find(name).GetComponent<AudioSource>());

            sfx[name].Play();
        }
        catch
        {
            Debug.Log("No Audio name : " + name);
        }
    }
    public void StopSound(string name)
    {
        try
        {
            if (!sfx.ContainsKey(name))
                sfx.Add(name, _SoundSFX.Find(name).GetComponent<AudioSource>());

            sfx[name].Stop();
        }
        catch
        {
            Debug.Log("No Audio name : " + name);
        }
    }
}
