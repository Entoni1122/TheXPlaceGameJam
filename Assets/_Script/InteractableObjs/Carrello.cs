using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrello : BaseInteractableObj
{
    private Rigidbody _rb;
    public Transform handlePosition;
    public float interactionDistance = 1.0f;
    private Inventory _inventory;
    public Vector3 playerOffset = new Vector3(0, 0, -1);
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    private Vector3 initialPlayerOffset = Vector3.zero;

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
        _rb = GetComponent<Rigidbody>();
    }

    protected override void InteractOneParam(Transform obj)
    {
        _inventory.AddObjInInventory(obj);
    }

    public void Handle(Transform player)
    {
        if (IsPlayerNearHandle(player))
        {
            if (initialPlayerOffset == Vector3.zero) 
            {
                initialPlayerOffset = player.position - handlePosition.position;
            }

            _rb.isKinematic = true;

            Vector3 newPosition = handlePosition.position + initialPlayerOffset;
            player.position = newPosition;

            Quaternion newRotation = handlePosition.rotation * Quaternion.Euler(rotationOffset);
            player.rotation = newRotation;

            transform.SetParent(player);
        }
    }

    public void Release(Transform player)
    {
        _rb.isKinematic = false;
        transform.SetParent(null);
        initialPlayerOffset = Vector3.zero; 
    }

    private bool IsPlayerNearHandle(Transform player)
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, handlePosition.position);
            return distance <= interactionDistance;
        }
        return false;
    }
}
