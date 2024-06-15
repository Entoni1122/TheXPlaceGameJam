using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulinoInteract : BaseInteractableObj
{
    [SerializeField] Transform postionToSit;
    protected override void InteractOneParam(Transform obj)
    {
        PlayerStats.OnEnableController?.Invoke(false, postionToSit,true);
        obj.parent = gameObject.transform;
        GetComponent<MulinoController>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("MulinoMotor");
    }
}
