using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    [SerializeField]
    Animator _animator;

    [SerializeField]
    float _attackRange = 5;   
    
    [SerializeField]
    float _WaitTime = 0.1f;

    public Enemy_Animation_Controller _Animation_Controller;
    public GameObject _attackHitBox;
    Transform _Target;
    Coroutine UnTarget_Coroutine;
    bool isBusy = false;
    void Start()
    {
        _attackHitBox.SetActive(false);
        StartCoroutine(Chasing());
    }
    IEnumerator Chasing()
    {
        while (true)
        {
            yield return new WaitUntil(() => _Target != null);
            while (_Target != null)
            {
                if (_Animation_Controller.isBusy)
                {
                    yield return new WaitWhile(() => _Animation_Controller.isBusy);
                    _attackHitBox.SetActive(false);
                    _animator.SetBool(Enemy_Animation.Attack, false);
                    float speed = _animator.speed;
                    _animator.speed = 0;
                    yield return new WaitForSeconds(_WaitTime);
                    _animator.speed = speed;
                }

                _animator.SetBool(Enemy_Animation.Walk, true);
                yield return new WaitForEndOfFrame();
            }
            _animator.SetBool(Enemy_Animation.Walk, false);
        }
    }
    IEnumerator UnChasing()
    {
        yield return new WaitForSeconds(10f);
        _Target = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(UnTarget_Coroutine != null) StopCoroutine(UnTarget_Coroutine);
            if (_Target == null) _animator.SetTrigger(Enemy_Animation.Taunt);
            _Target = other.transform;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 p_dist = other.transform.position;
            p_dist.y = 0;

            Vector3 e_dist = transform.position;
            e_dist.y = 0;

            if(Vector3.Distance(e_dist, p_dist) < _attackRange)
            {
                _animator.SetBool(Enemy_Animation.Walk, false);
                _animator.SetBool(Enemy_Animation.Attack, true);
                _attackHitBox.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UnTarget_Coroutine = StartCoroutine(UnChasing());
        }
    }

}
public class Enemy_Animation
{
    public static string Walk = "Walk";
    public static string Attack = "Attack";
    public static string Die = "Die";
    public static string Taunt = "Taunt";
}
