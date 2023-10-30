using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2_Controller : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField]
    public Animator _animator;
    public Enemy2_Animation_Controller _Animation_Controller;

    [Header("Attack")]
    [SerializeField] GameObject _Rock;

    [Header("Score")]
    [SerializeField] int _Score = 20;

    [SerializeField] GameObject _hitParticel;
    [HideInInspector] public Transform _Target;
    [HideInInspector] public Transform _transform;
    [HideInInspector] public Coroutine UnTarget_Coroutine;

    [HideInInspector] public bool died = false;

    void Start()
    {
        _transform = transform;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (died) return;
        if (collision.collider.CompareTag("Player_Attack"))
        {
            GoDie();
        }
    }

    private void Update()
    {
        _animator.SetBool("Target", _Target != null);
        if(_Target != null && !died)
        {
            var lookTo = _Target.position;
            lookTo.y = _transform.position.y;
            _transform.LookAt(lookTo);
        }
    }

    public IEnumerator UnTarget()
    {
        yield return new WaitForSeconds(2);
        _Target = null;
    }

    public void ThrowRock()
    {
        Destroy(Instantiate(_Rock, _transform.GetChild(0).Find("Rock").position, _transform.rotation), 5);
        Debug.Log("ThrowRock");
    }

    public void GoDie()
    {
        FindFirstObjectByType<Player_Control>()._score += _Score;
        Instantiate(_hitParticel, transform.position, Quaternion.identity, transform);
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
}
