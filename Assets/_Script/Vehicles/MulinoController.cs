using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class MulinoController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rb;

    [Header("Car Setup")]
    [SerializeField] float forwardAccel = 8f;
    [SerializeField] float reverseAccel = 4f;
    [SerializeField] float turnStrength = 180;
    [SerializeField] float carControlInputOnGround = 3.5f;

    [Header("Ground Check Info")]
    [SerializeField] Transform groundRayPoint;

    private float speedInput, turnInput, verticalInput;
    [SerializeField] bool bEnableController = false;
    public bool BEnableController { get { return bEnableController; } set { bEnableController = value; } }

    [SerializeField] float respawnTimer = 2f;
    bool isGrounded;
    [SerializeField] Transform playerDismountPos;
    private void Update()
    {
        if (bEnableController)
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 2f);
            InputReader();
        
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            PlayerStats.OnEnableController?.Invoke(true, playerDismountPos.position);

            gameObject.layer = LayerMask.NameToLayer("Interactable");
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (bEnableController)
        {
            if (!isGrounded)
            {
                return;
            }
            Move();
        }
    }
    private void InputReader()
    {
        float localVerticalInput = Input.GetAxisRaw("Vertical");
        float localTurnInput = Input.GetAxisRaw("Horizontal");

        verticalInput = localVerticalInput != 0 ? localVerticalInput : Mathf.Lerp(verticalInput, 0, Time.deltaTime * carControlInputOnGround);
        turnInput = localTurnInput != 0 ? localTurnInput : Mathf.Lerp(turnInput, 0, Time.deltaTime * carControlInputOnGround);

        OnVerticalInput();
        OnTurnInput();
    }
    private void OnVerticalInput()
    {
        speedInput = 0f;

        if (verticalInput != 0)
        {
            speedInput = verticalInput * (speedInput > 0 ? forwardAccel : reverseAccel);
        }
    }
    private void OnTurnInput()
    {
        if (isGrounded)
        {
            transform.Rotate(0, turnInput * turnStrength * Time.deltaTime, 0, Space.Self);
            return;
        }
    }
    private void Move()
    {
        if (Mathf.Abs(speedInput) > 0)
        {
            rb.AddForce(transform.forward * speedInput, ForceMode.Acceleration);
        }
    }
}
