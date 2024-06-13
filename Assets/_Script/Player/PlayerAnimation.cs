using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    FPController playerRef;
    Animator _animInstance;
    Rigidbody _rb;

    [SerializeField] string _isMovingID;
    [SerializeField] string _isFallingID;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb is null)
        {
            Debug.LogError("No Rigidbody in this object");
            Destroy(this);
        }


        _animInstance = GetComponent<Animator>();
        if (_animInstance is null)
        {
            Debug.LogError("No Animator in this object");
            Destroy(this);
        }


        playerRef = GetComponent<FPController>();
        if (playerRef is null)
        {
            Debug.LogError("No PhysicPlayerController in this object");
            Destroy(this);
        }
    }


    private void Update()
    {
        UpdateMoving();
        UpdateIsFalling();
    }
    private void UpdateMoving()
    {
        Vector2 input = new Vector2(playerRef.inputMove.x, playerRef.inputMove.z);
        if (input != Vector2.zero)
        {
            if (!_animInstance.GetBool(_isMovingID))
            {
                _animInstance.SetBool(_isMovingID, true);
            }
        }
        else
        {
            if (_animInstance.GetBool(_isMovingID))
            {
                _animInstance.SetBool(_isMovingID, false);
            }
        }
    }

    private void UpdateIsFalling()
    {
        if (!playerRef.isGrounded)
        {
            if (!_animInstance.GetBool(_isFallingID))
            {
                _animInstance.SetBool(_isFallingID, true);
            }
        }
        else
        {
            if (_animInstance.GetBool(_isFallingID))
            {
                _animInstance.SetBool(_isFallingID, false);
            }
        }
    }
}
