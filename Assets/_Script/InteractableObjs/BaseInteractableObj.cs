using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractableObj : MonoBehaviour, IInteract
{
    private Rigidbody _rb;
    [SerializeField] Vector3 objectOffset = Vector3.zero;

    private Point currentPoint;

    public static event Action<Point> OnObjectRemoved;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetCurrentPoint(Point point)
    {
        currentPoint = point;
    }

    void IInteract.Interact(Transform socket)
    {
        NotifyObjectRemoved();
        transform.parent = socket;
        transform.position = transform.parent.position + objectOffset;
        _rb.isKinematic = true;
    }

    void IInteract.Interact(Transform socket, Vector3 offset, int length)
    {
        NotifyObjectRemoved();
        transform.parent = socket;
        transform.position = transform.parent.position + offset + objectOffset * length;
        _rb.isKinematic = true;
    }

    void IInteract.ThrowAway(Vector3 impluseForce)
    {
        NotifyObjectRemoved();
        transform.parent = null;
        _rb.isKinematic = false;
        _rb.AddForce(impluseForce, ForceMode.Impulse);
    }

    private void NotifyObjectRemoved()
    {
        if (currentPoint != null)
        {
            currentPoint.isOccupied = false;
            OnObjectRemoved?.Invoke(currentPoint);
            currentPoint = null;
        }
    }
}