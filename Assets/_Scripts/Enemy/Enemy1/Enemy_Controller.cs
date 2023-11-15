using System.Collections;

using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField]
    public Animator _animator;
    public Enemy_Animation_Controller _Animation_Controller;

    [Header("Attack")]
    [SerializeField] GameObject _hitParticel;

    [Header("Movement")]
    [SerializeField] Transform _PatrolPaths;
    [SerializeField] NavMeshAgent _Agent;

    [Header("Score")]
    [SerializeField] int _Score = 30;

    [HideInInspector] public Transform _Target;
    public Transform _transform;
    [HideInInspector] public bool died = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (died) return;
        if (collision.collider.CompareTag("Player_Attack"))
        {
            Destroy(collision.gameObject);
            GoDie();
        }
    }

    public void GoDie()
    {
        FindFirstObjectByType<Player_Control>()._score += _Score;
        _Agent.isStopped = true;
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

    public void setPatrolPath(Transform patrolPath)
    {
        _PatrolPaths = patrolPath;
    }

    private void Update()
    {
        if(died) return;
        Move();
    }

    public void Move()
    {
        if (_Animation_Controller.isBusy)
        {
            _Agent.isStopped = true;
            return;
        }

        if (_Target != null && IsDestinationReachable(_Target.position))
            onChase();
        else
            onPatrol();
    }

    Transform PosPatrol;
    bool onChasing = false;
    public void onPatrol()
    {
        onChasing = false;
        if(_Agent.remainingDistance < .5f)
        {
            PosPatrol = _PatrolPaths.GetChild(Random.Range(0, _PatrolPaths.childCount));
        }
        Vector3 Location = PosPatrol.position;
        _Agent.SetDestination(Location);
        if (IsDestinationReachable(Location))
        {
            _Agent.isStopped = false;
            _animator.SetBool(Enemy_Animation.Walk, true);
        }
        else
        {
            _Agent.isStopped = true;
            _animator.SetBool(Enemy_Animation.Walk, false);
        }
    }
    public void onChase()
    {
        if (!onChasing)
        {
            _animator.SetTrigger("Taunt");
            onChasing = true;
            return;
        }
        
        _Agent.SetDestination(_Target.position);
        if (IsDestinationReachable(_Target.position))
        {
            _Agent.isStopped = false;
            _animator.SetBool(Enemy_Animation.Walk, true);
        }
        else
        {
            _Agent.isStopped = true;
            _animator.SetBool(Enemy_Animation.Walk, false);
        }
    }
    bool IsDestinationReachable(Vector3 destination)
    {
        // Perform a NavMeshPath calculation to check if the destination is reachable
        NavMeshPath path = new NavMeshPath();
        _Agent.CalculatePath(destination, path);

        // Check if the path status is not PathPartial (i.e., the destination is reachable)
        return path.status != NavMeshPathStatus.PathPartial;
    }
}
public class Enemy_Animation
{
    public static string Walk = "Walk";
    public static string Attack = "Attack";
    public static string Die = "Die";
    public static string Taunt = "Taunt";
}

