using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrello : BaseInteractableObj
{
    private Inventory _inventory;

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    protected override void InteractOneParam(Transform obj)
    {
        _inventory.AddObjInInventory(obj);
    }
}
