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
    [SerializeField] AttackStyle _attackStyle = AttackStyle.Melee;
    [SerializeField] GameObject _objectGun;
    [SerializeField] GameObject _Bullet;
    [SerializeField] bool _JumpShot = false;

    [Header("Movement")]
    [SerializeField]
    float _MovementSpeed = 5f;
    [SerializeField]
    float _IceLerpMovementSpeed = .5f;
    [SerializeField]
    float _character_rotate_speed = 10f;
    [SerializeField]
    float _floatingAmount = 0.1f;
    [SerializeField]
    float _minMovement = 0.05f;
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
    Transform _audioListener;
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
    public Player_Sound_Manager _PlayerSound;
    public GameObject _TookDamageParticle;
    public int _score = 0;
    public int _live = 3;
    game_manager _gm;

    Vector3 _closestEnemyPostition = Vector3.zero;
    Transform _player;
    Rigidbody _rb;
    bool _stun;
    [HideInInspector]
    public DateTime dateTime = DateTime.MinValue;

    public StepBlock _currentBlock = StepBlock.normal;

    public enum StepBlock
    {
        normal, ice, air
    }
    public enum AttackStyle
    {
        Melee, Gun
    }
    void Start()
    {
        _gm = FindFirstObjectByType<game_manager>();
        _rb = GetComponent<Rigidbody>();
        _player = transform;
        if (_camera == null) _camera = Camera.main.transform;
        _camera.position += Vector3.one;
        _camera.rotation = _player.rotation;
        initialPosition = transform.position;
    }

    void Update()
    {
        if (_gm.isPause) return;
        movecam();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (!_animation._Current_animation.busy)
        {
            JumpingOnInput();
            Attack();
        }
    }


    private void FixedUpdate()
    {
        if (_gm.isPause) return;
        VisionFace();
        //AimEnemy(); // BROKE AAAAAAAAAAAAAAAAAA
        _EnemyList.Clear();
        //stuckInGround();
        GroundCheck();
        Gravity();
        isPlayerFreeFalling();
        airControl();
        minMovement();
        if (!_animation._Current_animation.busy)
        {
            Moving_Control(Time.fixedDeltaTime);
        }
    }

    private void minMovement()
    {
        if (_rb.velocity.magnitude < _minMovement) _rb.velocity = Vector3.zero;
    }

    private void airControl()
    {
        if (_currentBlock == StepBlock.normal && !_stun)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
    }

    private void Attack()
    {
        switch (_attackStyle)
        {
            case AttackStyle.Melee:
                _objectGun.SetActive(false);
                MeleeAttack();
                break;
            case AttackStyle.Gun:
                _objectGun.SetActive(true);
                ShotingAttack();
                break;
        }
    }

    private void MeleeAttack()
    {
        if (Input.GetButtonDown("Fire1")) _animation.SpinAttack();
    }

    void ShotingAttack()
    {
        if (Input.GetButtonDown("Fire1") && !_animation._Current_animation.shoting)
        {
            if (!_grounded && !_JumpShot || _animation._Current_animation.jumping) return;
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
            var bullet = _Bullet;
            var bulletEulerAngle = bullet.transform.eulerAngles;
            bullet.transform.rotation = Quaternion.Euler(bulletEulerAngle.x, bulletEulerAngle.y - 90, bulletEulerAngle.z);
            Destroy(Instantiate(bullet, _player.position + _player.forward, _player.rotation), 5f);
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
        var halfE = new Vector3(_player.localScale.x * 0.8f, _player.localScale.y * 0.2f, _player.localScale.z * 0.8f);
        _grounded = Physics.BoxCast(ray.origin, halfE, Vector3.down, out RaycastHit hit, Quaternion.identity, _GroundDetectLength, _GroundlayerMask);

        //make it float a bit
        if (_grounded)
        {
            _player.Translate(Vector3.up * _floatingAmount);
        }

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
        _camera.rotation = _audioListener.rotation = _camVision.rotation = rotation;

        Vector3 desiredPosition = (_player.position - rotation * Vector3.forward * _cameraDistance) + _camOffset;

        _camera.position = _camVision.position = Vector3.Lerp(_camera.position, desiredPosition, 1);
    }
    float horizontal = 0;
    float vertical = 0;
    void Moving_Control(float time)
    {
        Vector3 cameraFwd = _camera.forward;
        cameraFwd.y = 0;
        cameraFwd.Normalize();

        float _MovementSpeed = this._MovementSpeed;

        if (_currentBlock == StepBlock.ice)
        {
            var vel = _rb.velocity;
            vel.y = 0;
            _MovementSpeed = Mathf.MoveTowards(vel.magnitude, this._MovementSpeed, _IceLerpMovementSpeed * time);
        }
        //Debug.Log(_MovementSpeed);
        Vector3 moveDirection = _MovementSpeed * (cameraFwd * vertical + _camera.right * horizontal).normalized;
        /*if (_currentBlock == StepBlock.ice && moveDirection != Vector3.zero)
        {
            var vel = _rb.velocity;
            vel.y = 0;
            moveDirection = Vector3.MoveTowards(vel, moveDirection, _IceLerpMovementSpeed * time);
        }*/
        //Debug.Log(moveDirection);
        bool isMoving = moveDirection != Vector3.zero;
        _animation.Moving(moveDirection.magnitude > 1f || isMoving);

        // movement
        if (!isMoving) return;
        _rb.velocity = new Vector3(moveDirection.x, _rb.velocity.y, moveDirection.z);

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
        //else _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
    }
    List<Transform> _EnemyList = new List<Transform>();
    public void AddEnemyList(Transform transform)
    {
        if (!_EnemyList.Contains(transform))
            _EnemyList.Add(transform);
    }
    public void RemoveEnemyList(Transform transform)
    {
        if (_EnemyList.Contains(transform))
            _EnemyList.Remove(transform);
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
                var direction = (enemy.position - _player.position).normalized;
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
            catch (Exception e)
            {
                Debug.LogError(e);
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
        if (collision.collider.CompareTag("IceGround"))
        {
            _currentBlock = StepBlock.ice;
        }
        else // Ground
        {
            _currentBlock = StepBlock.normal;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        /*
        if(collision.collider.CompareTag("Ground") && (collision.relativeVelocity.magnitude > 15f))
        {
            Debug.Log($"Stuck with {collision.relativeVelocity.magnitude} magnitude");
            _player.Translate(Vector3.up * Time.deltaTime);
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Collectable>(out Collectable collectable))
        {
            switch (collectable.collectedType)
            {
                case Collectable.CollectedType.coin:
                    _score += (int)collectable.value;
                    break;
                case Collectable.CollectedType.gun:
                    _attackStyle = AttackStyle.Gun;
                    break;
            }
            collectable.Collected();
            _PlayerSound.CollectSound();
        }
        if (other.CompareTag("Finish"))
        {
            _gm.GameFinish();
        }
    }
    IEnumerator TakeDamage(Transform source, float knockPower)
    {
        _PlayerSound.HurtSound();
        _stun = true;
        _animation.Hit(true);
        _animation.Moving(false);
        _grounded = false;
        Instantiate(_TookDamageParticle, _player.position, Quaternion.identity, _player);
        Vector3 knockDirection = (_player.position - source.position).normalized;
        knockDirection.y = 0;
        _rb.velocity = (knockDirection + Vector3.up) * knockPower;
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _grounded);
        _animation.Hit(false);
        _stun = false;
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
            if(_live < 0)
            {
                _gm.GameOver();
            }
        }
        if (_rb.velocity.y <= -_VelosityMax)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, -_VelosityMax, _rb.velocity.z);
        }
    }

}
