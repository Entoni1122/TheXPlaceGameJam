using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : BaseInteractableObj
{
    [SerializeField] GameObject shopUI;

    [SerializeField] GameObject padlockClosed;
    [SerializeField] GameObject padlockOpen;


    private void Start()
    {
        GameManager.OnIntermissionCall += FlipFlopPadlock;
    }

    private void FlipFlopPadlock(bool open)
    {
        padlockClosed.SetActive(!open);
        padlockOpen.SetActive(open);

        gameObject.layer = open ? LayerMask.NameToLayer("Interactable") : 0;
    }

    protected override void InteractNoParam()
    {
        shopUI.SetActive(true);
    }
}
