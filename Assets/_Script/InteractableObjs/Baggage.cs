using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baggage : BaseInteractableObj
{
    private Rigidbody _rb;
    private Point currentPoint;
    public static event Action<Point> OnObjectRemoved;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    protected override void InteractNoParam()
    {
        NotifyObjectRemoved();
        _rb.constraints = RigidbodyConstraints.FreezeAll;
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
