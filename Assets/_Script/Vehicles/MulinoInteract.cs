using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulinoInteract : BaseInteractableObj
{
    [SerializeField] Transform postionToSit;
    protected override void InteractOneParam(Transform obj)
    {
        print("Interact with mulino");
        PlayerStats.OnEnableController?.Invoke(false, postionToSit.position);
        obj.parent = gameObject.transform;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Rigidbody>().useGravity = false;
        GetComponent<MulinoController>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("MulinoMotor");
    }
}
