using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Player_Control : MonoBehaviour
{

    [SerializeField]
    public Player_Animation_Control _animation;

    [SerializeField]
    float _KnockPower = 10f;

    [Header("Attack")]
    [SerializeField] bool _allowGun = true;
    [SerializeField] GameObject _objectGun;
    [SerializeField] GameObject _Bullet;
    [SerializeField] bool _JumpShot = false;

    [Header("Movement")]
    [SerializeField]
    float _MovementSpeed = 5f;
    [SerializeField]
    float _character_rotate_speed = 10f;
    [SerializeField]
    float _jumpForce = 5f;
    [SerializeField]
    LayerMask _GroundlayerMask;
    [SerializeField]
    float _GroundDetectLength = 2.5f;
    public bool _grounded = true;
    [SerializeField]
    float _GravityPower = 1.5f;
    [SerializeField]
    float _VelosityMax = 100f;

    [Header("Camera")]
    [SerializeField]
    Transform _camera;
    [SerializeField]
    float _cameraDistance = 5f;
    public float _cameraSensitivity = 4f;
    [SerializeField]
    Vector3 _camOffset;
    [SerializeField]
    Transform _camVision;
    [SerializeField]
    Transform _faceVision;

    [Header("Position")]
    public Vector3 initialPosition;

    [Header("Etc.")]
    public GameObject _TookDamageParticle;
    public int _score = 0;
    public int _live = 3;

    Vector3 _closestEnemyPostition = Vector3.zero;
    Transform _player;
    Rigidbody _rb;
    bool _stun;
    [HideInInspector]
    public DateTime dateTime = DateTime.MinValue;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = transform;
        if (_camera == null) _camera = Camera.main.transform;
        initialPosition = transform.position;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        movecam();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (!_animation._Current_animation.busy)
        {
            JumpingOnInput();
            Shoting();
        }
    }


    private void FixedUpdate()
    {
        VisionFace();
        AimEnemy();
        _EnemyList.Clear();
        GroundCheck();
        Gravity();
        isPlayerFreeFalling();
        if (!_animation._Current_animation.busy)
        {
            Moving_Control(Time.fixedDeltaTime);
        }
    }


    void Shoting()
    {
        if (!_allowGun)
        {
            _objectGun.SetActive(false);
            return;
        }
        else if (!_objectGun.activeSelf) _objectGun.SetActive(true);
        if (Input.GetButtonDown("Fire1") && !_animation._Current_animation.shoting)
        {
            if (!_grounded && !_JumpShot) return;
            if (dateTime == DateTime.MinValue)
            {
                Debug.Log("Shot");
                dateTime = DateTime.Now;
            }
            // rotate toward
            Vector3 target = Vector3.zero;
            if (_closestEnemyPostition == Vector3.zero)
            {
                target = _player.position - _camera.position;
            }
            else target = _closestEnemyPostition - _player.position;

            _rb.velocity = Vector3.zero;
            Quaternion rot = Quaternion.LookRotation(target);
            Quaternion lookTo = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            _player.rotation = lookTo;

            // summon bullet
            Destroy(Instantiate(_Bullet, _player.position, _player.rotation), 5f);
            _animation.Shot();
        }

    }
    void VisionFace()
    {
        _faceVision.position = _player.position;
        _faceVision.rotation = _player.rotation;
    }
    private void GroundCheck()
    {
        Ray ray = new Ray(_player.position, Vector3.down);
        _grounded = Physics.BoxCast(ray.origin, new Vector3(2, 0.5f, 2), Vector3.down, out RaycastHit hit, Quaternion.identity, _GroundDetectLength, _GroundlayerMask) && hit.collider.tag == "Ground";

        _animation.Grounded(_grounded);
    }

    private void JumpingOnInput()
    {
        if (Input.GetButtonDown("Jump") && _grounded)
        {
            if (dateTime == DateTime.MinValue)
            {
                Debug.Log("Jump");
                dateTime = DateTime.Now;
            }
            jump(_jumpForce);
        }
    }

    public void jump(float jumpForce)
    {
        var vel = _rb.velocity;
        vel.y = 0;
        _rb.velocity = vel + Vector3.up * jumpForce;
        _animation.Jump();
    }


    void movecam()
    {

        float mouseX = Input.GetAxis("Mouse X") * _cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _cameraSensitivity;

        float currentRotationX = _camera.eulerAngles.x;

        currentRotationX -= mouseY;
        currentRotationX = Mathf.Clamp(currentRotationX, 0, 70);

        Quaternion rotation = Quaternion.Euler(currentRotationX, _camera.eulerAngles.y + mouseX, 0);
        _camera.rotation = _camVision.rotation = rotation;

        Vector3 desiredPosition = (_player.position - rotation * Vector3.forward * _cameraDistance) + _camOffset;

        _camera.position = _camVision.position = Vector3.Lerp(_camera.position, desiredPosition, 1f);
    }
    float horizontal = 0;
    float vertical = 0;
    void Moving_Control(float time)
    {
        // if (_animation._Current_animation == PlayerAnimation.shoting || _stun) return;

        Vector3 cameraFwd = _camera.forward;
        cameraFwd.y = 0;
        cameraFwd.Normalize();

        Vector3 moveDirection = cameraFwd * vertical + _camera.right * horizontal;

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }
        bool isMoving = moveDirection != Vector3.zero;
        _animation.Moving(moveDirection.magnitude > 1f || isMoving);

        // movement
        if (!isMoving) return;
        _rb.MovePosition(_player.position + (_MovementSpeed * time * moveDirection));

        Quaternion rotation = Quaternion.LookRotation(moveDirection);
        _player.rotation = Quaternion.Slerp(_player.rotation, rotation, time * _character_rotate_speed);
        if (dateTime == DateTime.MinValue)
        {
            Debug.Log("Move");
            dateTime = DateTime.Now;
        }
    }
    void Gravity()
    {
        if (!_grounded) _rb.velocity += Vector3.down * _GravityPower;
    }
    List<Transform> _EnemyList = new List<Transform>();
    public void AddEnemyList(Transform transform)
    {
        if (!_EnemyList.Contains(transform))
            _EnemyList.Add(transform);
    }
    private void AimEnemy()
    {
        float closest_Enemy_dist = float.MaxValue;
        Vector3 closest_Enemy = Vector3.zero;
        foreach (Transform enemy in _EnemyList)
        {
            // Check FOV
            try
            {
                var direction = enemy.position - _player.position;
                Ray ray = new Ray(_player.position, direction);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Enemy"))
                {
                    Debug.DrawRay(_player.position, direction);
                    // Get Closest
                    float dist = Vector3.Distance(enemy.position, _player.position);
                    if (dist < closest_Enemy_dist)
                    {
                        closest_Enemy_dist = dist;
                        closest_Enemy = enemy.position;
                    }
                }
            }
            catch
            {

            }
        }
        _closestEnemyPostition = closest_Enemy;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Damage") || collision.collider.CompareTag("Enemy"))
        {
            StartCoroutine(TakeDamage(collision.transform, _KnockPower));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            Debug.Log($"Took damage from {other.name}");
            StartCoroutine(TakeDamage(other.transform, _KnockPower / 2));
        }
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"Took damage from {other.name}");
            StartCoroutine(TakeDamage(other.transform, _KnockPower));
        }
        if (other.TryGetComponent<Coin>(out Coin coin))
        {
            coin.Collected();
            _score += (int)coin.value;
        }
        if (other.CompareTag("Finish"))
        {
            FindFirstObjectByType<game_manager>().GameFinish("Level Complete", true);
        }
    }
    IEnumerator TakeDamage(Transform source, float knockPower)
    {
        _animation.Hit(true);
        _animation.Moving(false);
        _grounded = false;
        Instantiate(_TookDamageParticle, _player.position, Quaternion.identity, _player);
        Vector3 knockDirection = (_player.position - source.position);
        if (knockDirection.magnitude > 1f)
        {
            knockDirection.Normalize();
        }

        _rb.velocity = (knockDirection + Vector3.up) * knockPower;
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _grounded);
        _animation.Hit(false);
    }

    private void isPlayerFreeFalling()
    {
        if (transform.position.y < initialPosition.y - 40f)
        {
            //_animation.Moving(false);
            transform.position = initialPosition;
            _rb.velocity = Vector3.zero;
            _animation.resetAnimation();
            _live--;
        }
        if (_rb.velocity.y < -_VelosityMax)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, -_VelosityMax, _rb.velocity.z);
        }
    }

}
