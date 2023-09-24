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
    Player_Animation_Control _animation;
    [SerializeField]
    float _GravityPower = 1.5f;
    [SerializeField]
    float _KnockPower =10f;
    [Header("Movement")]
    [SerializeField]
    float _MovementSpeed = 5f;
    [SerializeField]
    float _character_rotate_speed = 10f;
    [SerializeField]
    float _jumpForce = 5f;
    [SerializeField]
    LayerMask _layerMask;

    [Header("Camera")]
    [SerializeField]
    Transform _camera;
    [SerializeField]
    float _cameraDistance = 5f;
    public float _cameraSensitivity = 4f;

    bool _grounded = true;
    Transform _player;
    Rigidbody _rb;
    bool _stun;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = transform;
        //if (_camera == null) _camera = Camera.main.transform;
    }

    void Update()
    {
        movecam();
        Jumping();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        GroundCheck();
        Gravity();
        Moving_Control(Time.fixedDeltaTime);
        
    }
    private void GroundCheck()
    {
        Ray ray = new Ray(_player.position, Vector3.down);
        _grounded = Physics.BoxCast(ray.origin, new Vector3(2, 0.5f, 2), Vector3.down, out RaycastHit hit, Quaternion.identity, 2.5f, _layerMask) && hit.collider.tag == "Ground";
        _animation.Grounded(_grounded);
    }

    private void Jumping()
    {
        if (Input.GetButtonDown("Jump") && _grounded)
        {
            _rb.velocity = _rb.velocity + Vector3.up * _jumpForce;
            _animation.Jump();
        }
    }


    void movecam()
    {
        float mouseX = Input.GetAxis("Mouse X") * _cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _cameraSensitivity;

        float currentRotationX = _camera.transform.eulerAngles.x;

        currentRotationX -= mouseY;
        currentRotationX = Mathf.Clamp(currentRotationX, 0, 70);

        Quaternion rotation = Quaternion.Euler(currentRotationX, _camera.transform.eulerAngles.y + mouseX, 0);
        _camera.transform.rotation = rotation;

        Vector3 desiredPosition = _player.position - rotation * Vector3.forward * _cameraDistance;

        _camera.transform.position = desiredPosition;
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

        if (!isMoving) return;
        _player.Translate(moveDirection * _MovementSpeed * time, Space.World);

        Quaternion rotation = Quaternion.LookRotation(moveDirection);
        _player.rotation = Quaternion.Slerp(_player.rotation, rotation, Time.deltaTime * _character_rotate_speed);
    }
    void Gravity()
    {
        if (!_grounded) _rb.velocity += Vector3.down * _GravityPower;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Damage")
        {
            TakeDamage(collision.transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
    }

    private void TakeDamage(Transform source)
    {
        _animation.Hit();
        Vector3 knockDirection = (_player.position - source.position);
        if (knockDirection.magnitude > 1f)
        {
            knockDirection.Normalize();
        }

        _rb.velocity = (knockDirection + Vector3.up) * _KnockPower;
    }

}
