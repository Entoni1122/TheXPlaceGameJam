using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baggage : BaseInteractableObj
{
    private Rigidbody _rb;
    private Point currentPoint;
    public static event Action<Point> OnObjectRemoved;
    [SerializeField] GameObject vfxTrail;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    protected override void InteractNoParam()
    {
        NotifyObjectRemoved();
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        DisableTrail();
        GetComponent<EntityProp>().ResetMoveAction();
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

    public override void ThrowAway(Vector3 force)
    {
        base.ThrowAway(force);

        if (vfxTrail)
        {
            vfxTrail.GetComponentInChildren<TrailRenderer>().emitting = true;
            Invoke("DisableTrail", 1f);
        }
    }
    private void DisableTrail()
    {
        if (vfxTrail)
        {
            vfxTrail.GetComponentInChildren<TrailRenderer>().emitting = false;
        }
    }
}
