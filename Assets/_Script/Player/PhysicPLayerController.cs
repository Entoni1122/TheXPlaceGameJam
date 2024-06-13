using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PhysicPLayerController : MonoBehaviour
{
    [SerializeField] Transform positionToSpawn;

    [Header("Movemnet")]
    [SerializeField] float _speed = 5f;
    [SerializeField] float _turnSpeed = 360f;

    [Header("Jump")]
    [SerializeField] float _jumpForce = 10f;
    [SerializeField] float gravity = 20f;

    Rigidbody _rb;
    [SerializeField] bool isGrounded;

    public bool IsGrounded => isGrounded;
    private Vector3 _input;
    public Vector3 InputRead => _input;

    [SerializeField] bool bCanRespawn;
    bool enableContorller = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (bCanRespawn)
        {
            bCanRespawn = false;
            ResetPosition();
        }

        if (enableContorller)
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
    }

    void ResetPosition()
    {
        transform.position = positionToSpawn.position;
        enableContorller = true;
        _rb.freezeRotation = true;
        transform.rotation = Quaternion.identity;
    }
    private void FixedUpdate()
    {
        if (enableContorller)
        {
            Move();
        }
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    #region Movement
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
    #endregion

    [SerializeField] float explosionForce = 200f;
    [SerializeField] float explosionRadius = 20f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Autobus"))
        {
            _rb.AddExplosionForce(explosionForce, collision.transform.position, explosionRadius);
            enableContorller = false;
            _rb.freezeRotation = false;
        }
    }
}
public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
