using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;



public class MulinoController : MonoBehaviour
{
    [Header("Movemnet")]
    [SerializeField] float _speed = 5f;
    [SerializeField] float _turnSpeed = 2f;
    private float currentSpeed;

    [Header("Jump")]
    [SerializeField] float gravity = 20f;

    Rigidbody _rb;
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundChecker;
    [SerializeField] Transform playerDismountPos;

    private Vector3 _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        currentSpeed = _speed;

        PlayerStats.OnChangeStats += (float inSpeedMultiplier, float InForce, bool InMagnetism) =>
        {
            currentSpeed = _speed * inSpeedMultiplier;
        };
    }

    private void Update()
    {
        GatherInput();
        Look();
        isGrounded = Physics.Raycast(groundChecker.position, -transform.up, 1f);
        if (!isGrounded)
        {
            _rb.velocity -= new Vector3(0, gravity, 0) * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            UnrealPlayerController controller = LibraryFunction.GetUnrealPlayerController();
            controller.EnableInput();
            controller.SetTransfrom(playerDismountPos);
            controller.GetComponent<PlayerAnimation>().NotifyOnCar(false);


            gameObject.layer = LayerMask.NameToLayer("Interactable");
            enabled = false;
        }

    }
    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    [Header("Exolosion when getting hitted")]
    [SerializeField] float explosionForce = 200f;
    [SerializeField] float explosionRadius = 20f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Autobus"))
        {
            _rb.AddExplosionForce(explosionForce, collision.transform.position, explosionRadius);
            _rb.freezeRotation = false;
            if (GetComponent<Inventory>())
            {
                GetComponent<Inventory>().ThrowAwayAllItems();
            }
        }
    }

    #region Movement
    private void Look()
    {
        if (_input.z == 0) return;

        transform.Rotate(0, _input.x * _turnSpeed * Time.deltaTime, 0, Space.Self);
    }

    private void Move()
    {
        if (!isGrounded) { return; }

        _rb.AddForce(transform.forward * currentSpeed * _input.z, ForceMode.Acceleration);
    }

    #endregion
}