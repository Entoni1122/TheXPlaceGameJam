using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baggage : BaseInteractableObj
{

    private Rigidbody _rb;
    [SerializeField] Vector3 objectOffset = Vector3.zero;

    private Point currentPoint;

    public static event Action<Point> OnObjectRemoved;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected override void InteractNoParam()
    {
        NotifyObjectRemoved();
        _rb.isKinematic = true;
    }

    public void NotifyObjectRemoved()
    {
        if (currentPoint != null)
        {
            currentPoint.isOccupied = false;
            OnObjectRemoved?.Invoke(currentPoint);
            currentPoint = null;
        }
    }
}
