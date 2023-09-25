using Palmmedia.ReportGenerator.Core.Parser.Analysis;
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
    public void Hit()
    {
        _animator.SetTrigger("Knock");
    }
    public void animation_event(string Event)
    {
        string[] _Event = Event.Split('_');
        Debug.Log($"Name {_Event[0]} | Type : {_Event[1]}");
        if(_Event[0] == "knock")
        {
            _Current_animation.knock  = _Current_animation.busy = Convert.ToBoolean(Convert.ToInt32(_Event[1]));
        }
        else if (_Event[0] == "Shoting")
        {
            _Current_animation.shoting = _Current_animation.busy = Convert.ToBoolean(Convert.ToInt32(_Event[1]));
        }
    }
    public void Shot()
    {
        _animator.SetTrigger("Shot");
    }
}
public class PlayerAnimation
{
    public bool idle, jumping, running, shoting, knock, busy;
}


