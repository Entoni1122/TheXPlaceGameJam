using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteract
{
    [SerializeField] GameObject shopUI;


    bool IInteract.Interact(Transform socket)
    {
        shopUI.SetActive(true);
        return false;
    }

    bool IInteract.Interact(Transform socket, Vector3 offset, int length)
    {
        shopUI.SetActive(true);
        return false;
    }

    void IInteract.ThrowAway(Vector3 impluseForce) { }
}
