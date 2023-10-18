using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Animation_Control : MonoBehaviour
{
    [SerializeField]
    Animator _animator;

    [HideInInspector]
    public Transform _transform;
    [HideInInspector]
    public GameObject _gameObject;
    [HideInInspector]
    public PlayerAnimation _Current_animation;

    void Start()
    {
        _Current_animation = new PlayerAnimation();
        _AutoResetAnim = StartCoroutine(AutoResetAnimation(0f));
    }
    private void Update()
    {
        _animator.SetBool("Busy", _Current_animation.busy);
    }
    public void Grounded(bool ground)
    {
        _animator.SetBool("Grounded", ground);
    }
    public void Moving(bool move)
    {
        _animator.SetBool("Moving", move);
    }
    public void Jump()
    {
        _animator.SetTrigger("Jumping");
        _animator.SetBool("Grounded", false);
    }
    public void Hit(bool knock)
    {
        _animator.SetBool("Knock", knock);
        _Current_animation.knock = _Current_animation.busy = knock;
    }
    Coroutine _AutoResetAnim;
    public void animation_event(string Event)
    {
        string[] _Event = Event.Split('_');
        Debug.Log($"Name {_Event[0]} | Type : {_Event[1]}");
        if (_Event[0] == "knock")
        {
            //_Current_animation.knock = _Current_animation.busy = Convert.ToBoolean(Convert.ToInt32(_Event[1]));
            //_animator.SetBool("Knock", _Current_animation.knock);
        }
        else if (_Event[0] == "Shoting")
        {
            _Current_animation.shoting = _Current_animation.busy = Convert.ToBoolean(Convert.ToInt32(_Event[1]));
        }

        //StopCoroutine(_AutoResetAnim);
        //_AutoResetAnim = StartCoroutine(AutoResetAnimation(1.5f));

    }
    IEnumerator AutoResetAnimation(float Time)
    {
        yield return new WaitForSeconds(Time);
        resetAnimation();
    }
    public void Shot()
    {
        _animator.SetTrigger("Shot");
    }
    public void resetAnimation()
    {
        _Current_animation.knock = _Current_animation.shoting = _Current_animation.busy = false;
    }
}
public class PlayerAnimation
{
    public bool idle, jumping, running, shoting, knock, busy;
}


