using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : BaseInteractableObj
{
    [SerializeField] GameObject shopUI;
    protected override void InteractNoParam()
    {
        shopUI.SetActive(true);
    }
}
