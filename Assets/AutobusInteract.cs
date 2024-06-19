using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutobusInteract : BaseInteractableObj
{
    [SerializeField] Transform postionToSit;
    protected override void InteractOneParam(Transform obj)
    {
        UnrealPlayerController controller = LibraryFunction.GetUnrealPlayerController();
        controller.DisableInput();
        controller.SetTransfrom(postionToSit);
        controller.GetComponent<PlayerAnimation>().NotifyOnCar(true);

        obj.parent = gameObject.transform;
        GetComponent<MulinoController>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("MulinoMotor");
    }

    private void OnLoseRound()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        GameManager.OnLoseRound += OnLoseRound;
    }
    private void OnDestroy()
    {
        GameManager.OnLoseRound -= OnLoseRound;
    }
    protected override void InteractTwoParam(Transform transform, bool bNig)
    {
        if (bNig)
        {
            GetComponent<Inventory>().AddObjInInventory(transform);
        }
    }
}