using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] MonoBehaviour playerRef;
    Inventory inventory;
    Animator _animInstance;
    Rigidbody _rb;

    [SerializeField] string _MovingID;
    [SerializeField] string _isFallingID;
    [SerializeField] string _OnCarID;
    [SerializeField] string _HandsUpID;
    [SerializeField] string _HandsOnCartID;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("No Rigidbody in this object");
            Destroy(this);
        }

        _animInstance = GetComponent<Animator>();
        if (_animInstance == null)
        {
            Debug.LogError("No Animator in this object");
            Destroy(this);
        }

        if (playerRef == null)
        {
            Debug.LogError("No controller in this object");
            Destroy(this);
        }

        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        UpdateMoving();
        UpdateIsFalling();
        NotifyHands(!inventory.IsEmpty);
    }
    private void UpdateMoving()
    {
        Vector2 input = new Vector2(GetInputMove().x, GetInputMove().z);
        input.Normalize();  
        _animInstance.SetFloat(_MovingID, input.magnitude,.1f,Time.deltaTime);
    }
    private void UpdateIsFalling()
    {
        if (!GetIsGrounded())
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
    public void NotifyOnCar(bool InCar)
    {
        _animInstance.SetBool(_OnCarID, InCar);
    }
    public void NotifyOnCart(bool InCart)
    {
        _animInstance.SetBool(_HandsOnCartID, InCart);
    }
    private void NotifyHands(bool handsUp)
    {
        _animInstance.SetBool(_HandsUpID, handsUp);
    }
    private bool GetIsGrounded()
    {
        Type type = playerRef.GetType();
        FieldInfo field = type.GetField("isGrounded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (field != null && field.FieldType == typeof(bool))
        {
            return (bool)field.GetValue(playerRef);
        }

        return false;
    }
    private Vector3 GetInputMove()
    {
        Type type = playerRef.GetType();
        FieldInfo field = type.GetField("_input", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (field != null && field.FieldType == typeof(Vector3))
        {
            return (Vector3)field.GetValue(playerRef);
        }

        return Vector3.zero;
    }
}
