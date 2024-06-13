using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variable")]
    [SerializeField] float speed = 10;
    [SerializeField] float rotationSpeed = 1;

    [Header("Jump + Gravity")]
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float groundedGravity = -0.2f;
    [SerializeField] float initualJumpVelocity;
    [SerializeField] float maxJumpHeght = 1f;
    [SerializeField] float maxJumpTime = 0.5f;

    [Header("GroundCheck")]
    [SerializeField] float rayCastGroundDistance = 2;
    [SerializeField] bool bIsGrounded;
    [SerializeField] bool bIsJumping;

    CharacterController _characterController;

    float moveInput;
    float turnInput;
    public Vector3 movementDirection;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        JumpVariables();
    }

    private void Update()
    {
        ReadInput();
        MoveCharacter();
        RotateCharacter();

        bIsGrounded = Physics.Raycast(transform.position, -transform.up, rayCastGroundDistance);


        Jump();
        ApplyGravity();
    }

    #region CharacterLocomotion
    void MoveCharacter()
    {
        if (bIsGrounded)
        {
            movementDirection = new Vector3(moveInput, 0, turnInput);
        }

        _characterController.Move(movementDirection * speed * Time.deltaTime);
    }
    void RotateCharacter()
    {
        Vector3 dirToLook;

        dirToLook.x = movementDirection.x;
        dirToLook.y = 0;
        dirToLook.z = movementDirection.z;
        Quaternion currentRot = transform.rotation;

        if (movementDirection != Vector3.zero)
        {
            Quaternion rotationToGo = Quaternion.LookRotation(dirToLook, Vector3.up);
            transform.rotation = Quaternion.Slerp(currentRot, rotationToGo, rotationSpeed);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && bIsGrounded && !bIsJumping)
        {
            bIsJumping = true;
            bIsGrounded = false;
            movementDirection.y = initualJumpVelocity;
        }
        else if (!Input.GetKeyDown(KeyCode.Space) && bIsJumping && bIsGrounded)
        {
            bIsJumping = false;
        }
    }
    #endregion


    #region SetUpVariables
    void JumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeght) / Mathf.Pow(timeToApex, 2);
        initualJumpVelocity = (2 * maxJumpHeght) / timeToApex;
    }
    #endregion


    #region Calculation
    void ReadInput()
    {
        moveInput = Input.GetAxis("Horizontal");
        turnInput = Input.GetAxis("Vertical");
    }
    void ApplyGravity()
    {
        bool IsFalling = movementDirection.y <= 0.0f;
        float multiplier = 2.0f;
        if (bIsGrounded)
        {
            movementDirection.y = groundedGravity;
        }
        else if (IsFalling)
        {
            float previousYVelocity = movementDirection.y;
            float newYVelo = movementDirection.y + (gravity * multiplier * Time.deltaTime);
            float nextVel = (previousYVelocity + newYVelo) * 0.5f;
            movementDirection.y = nextVel;
        }
        else
        {
            float previousYVelocity = movementDirection.y;
            float newYVelo = movementDirection.y + (gravity * Time.deltaTime);
            float nextVel = (previousYVelocity + newYVelo) * 0.5f;
            movementDirection.y = nextVel;
        }
    }
    #endregion  
}
