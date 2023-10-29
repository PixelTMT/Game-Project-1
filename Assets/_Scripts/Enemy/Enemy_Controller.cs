using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField]
    public Animator _animator;
    public Enemy_Animation_Controller _Animation_Controller;

    [Header("Attack")]
    [SerializeField]
    public float _attackRange = 5;
    [SerializeField]
    float _WaitTime = 0.1f;
    public GameObject _attackHitBox;
    [SerializeField] GameObject _hitParticel;

    [Header("Movement")]
    [SerializeField] float raycastDistance = 2.0f;
    [SerializeField] float _rotationSpeed = 8.0f;
    [SerializeField] float _MovementSpeed = 4f;
    [SerializeField] Transform _PatrolPaths;

    [Header("Score")]
    [SerializeField] int _Score = 30;

    [HideInInspector] public Transform _Target;
    public Transform _transform;
    [HideInInspector] public Coroutine UnTarget_Coroutine;
    [HideInInspector] public Coroutine _chasing, _patrol;
    [HideInInspector] public bool died = false;

    void Start()
    {
        _attackHitBox.SetActive(false);
        _chasing = StartCoroutine(Chasing());
        _patrol = StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        var waitFrame = new WaitForEndOfFrame();
        yield return new WaitForSeconds(0);
        if (_PatrolPaths != null && _PatrolPaths.childCount > 0)
        {
            int _currentPatrolPosition = 0, _PatrolDirection = 1;
            while (!died)
            {
                yield return new WaitWhile(() => _Target != null);

                if (_currentPatrolPosition < 0)
                {
                    _currentPatrolPosition = 0;
                    _PatrolDirection = 1;
                }
                else if (_currentPatrolPosition > _PatrolPaths.childCount - 1)
                {
                    _currentPatrolPosition = _PatrolPaths.childCount - 1;
                    _PatrolDirection = -1;
                }
                Transform Pos = _PatrolPaths.GetChild(_currentPatrolPosition);
                Vector3 Location = Pos.position;
                while (Vector3.Distance(_transform.position, Location) > 1 && _Target == null && !died)
                {
                    Location.y = _transform.position.y;
                    Vector3 LookDirection = Location - _transform.position;
                    Quaternion lookAt = Quaternion.LookRotation(LookDirection);
                    Quaternion rot = Quaternion.Lerp(_transform.rotation, lookAt, Time.deltaTime * _rotationSpeed);
                    _transform.rotation = rot;
                    _transform.Translate(Time.deltaTime * _MovementSpeed * Vector3.forward);
                    _animator.SetBool(Enemy_Animation.Walk, true);

                    yield return waitFrame;
                }
                _currentPatrolPosition += 1 * _PatrolDirection;

            }
        }
    }
    public IEnumerator Chasing()
    {
        var waitFrame = new WaitForEndOfFrame();
        var waitforSec = new WaitForSeconds(_WaitTime);
        yield return new WaitForSeconds(0);
        while (!died)
        {
            yield return new WaitUntil(() => _Target != null);
            _animator.SetTrigger(Enemy_Animation.Taunt);
            yield return new WaitWhile(() => _Animation_Controller.isBusy);
            while (_Target != null && !died)
            {
                //animation
                if (_Animation_Controller.isBusy)
                {
                    yield return new WaitWhile(() => _Animation_Controller.isBusy);
                    _attackHitBox.SetActive(false);
                    _animator.SetBool(Enemy_Animation.Attack, false);
                    float speed = _animator.speed;
                    _animator.speed = 0;
                    yield return waitforSec;
                    _animator.speed = speed;
                }
                _animator.SetBool(Enemy_Animation.Walk, true);


                bool isPathAvailable = isBlockInfrontExist();

                if (isPathAvailable && _Target != null)
                {
                    Vector3 Location = _Target.position;
                    Location.y = _transform.position.y;
                    Vector3 LookDirection = Location - _transform.position;
                    Quaternion lookAt = Quaternion.LookRotation(LookDirection);
                    Quaternion rot = Quaternion.Lerp(_transform.rotation, lookAt, Time.deltaTime * _rotationSpeed);
                    _transform.rotation = rot;
                    _transform.Translate(Time.deltaTime * _MovementSpeed * Vector3.forward);
                    _animator.SetBool(Enemy_Animation.Walk, true);
                }
                else
                {
                    _Target = null;
                }

                yield return waitFrame;
            }
            _animator.SetBool(Enemy_Animation.Walk, false);
        }
    }
    public IEnumerator UnChasing()
    {
        yield return new WaitForSeconds(10f);
        _Target = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (died) return;
        if (collision.collider.CompareTag("Player_Attack"))
        {
            GoDie();
        }
    }

    public void GoDie()
    {
        FindFirstObjectByType<Player_Control>()._score += _Score;
        Instantiate(_hitParticel, transform.position, Quaternion.identity, transform);
        if (_patrol != null) StopCoroutine(_patrol);
        if (_chasing != null) StopCoroutine(_chasing);
        Destroy(gameObject, 2f);
        _animator.SetBool(Enemy_Animation.Die, true);
        // disable collider
        GetComponentInChildren<BoxCollider>().enabled = false;
        died = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (died) return;
        if (other.CompareTag("Player_Attack"))
        {
            Debug.Log(other.name);
            GoDie();
        }
    }


    private bool isBlockInfrontExist()
    {
        Vector3 raycastDirection = Vector3.down;
        Vector3 offset = new Vector3(0, 0, 2.2f);
        Vector3 raycastOrigin = _transform.TransformPoint(offset);
        RaycastHit hit;

        Debug.DrawLine(transform.position, raycastDirection);

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true;
            }
        }

        return false;
    }

    public void setPatrolPath(Transform patrolPath)
    {
        _PatrolPaths = patrolPath;
    }

    private void Update()
    {
        Vector3 raycastDirection = Vector3.down;
        Vector3 offset = new Vector3(0, 0, 2.2f); // Adjust the values as needed to position it behind the character.
        Vector3 raycastOrigin = _transform.TransformPoint(offset);
        RaycastHit hit;

        Debug.DrawLine(raycastOrigin, raycastOrigin + raycastDirection * raycastDistance, Color.red);
    }

}
public class Enemy_Animation
{
    public static string Walk = "Walk";
    public static string Attack = "Attack";
    public static string Die = "Die";
    public static string Taunt = "Taunt";
}

