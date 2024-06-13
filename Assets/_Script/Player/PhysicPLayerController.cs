using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicPLayerController : MonoBehaviour
{
    [Header("Movemnet")]
    [SerializeField] float _speed = 5f;
    [SerializeField] float _turnSpeed = 360f;

    [Header("Jump")]
    [SerializeField] float _jumpForce = 10f;
    [SerializeField] float gravity = 20f;

    Rigidbody _rb;
    bool isGrounded;
    private Vector3 _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GatherInput();
        Look();
        isGrounded = Physics.Raycast(transform.position, -transform.up, 1f);
        if (isGrounded)
        {
            _rb.velocity -= new Vector3(0, 0, 0) * Time.deltaTime;
        }
        else
        {
            _rb.velocity -= new Vector3(0, gravity, 0) * Time.deltaTime;
        }
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        _rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _rb.velocity += new Vector3(0, _jumpForce, 0);
        }
    }
}
public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}

