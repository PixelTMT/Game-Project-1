using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sound_Manager : MonoBehaviour
{
    AudioSource _collect, _hurt, _spin, _jump, _walk;
    void Start()
    {
        Transform PlayerSFX = transform.Find("PlayerSFX");
        _collect = PlayerSFX.Find("Collect").GetComponent<AudioSource>();
        _hurt = PlayerSFX.Find("Hurt").GetComponent<AudioSource>();
        _spin = PlayerSFX.Find("Spin").GetComponent<AudioSource>();
        _jump = PlayerSFX.Find("Jump").GetComponent<AudioSource>();
        _walk = PlayerSFX.Find("Walk").GetComponent<AudioSource>();
    }

    public void CollectSound()
    {
        _collect.Play();
    }

    public void HurtSound()
    {
        _hurt.Play();
    }

    public void SpinSound()
    {
        _spin.Play();
    }

    public void JumpSound()
    {
        _jump.Play();
    }

    public void WalkSound()
    {
        _walk.Play();
    }
}
