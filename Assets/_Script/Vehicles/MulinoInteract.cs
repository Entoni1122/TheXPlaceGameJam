using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulinoInteract : BaseInteractableObj
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
        LibraryFunction.GetUnrealPlayerController().transform.parent = null;
        LibraryFunction.GetUnrealPlayerController().EnableInput();
        LibraryFunction.GetUnrealPlayerController().GetComponent<PlayerAnimation>().NotifyOnCar(false);
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
}