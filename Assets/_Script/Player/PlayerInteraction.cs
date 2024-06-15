using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using System;

enum PlayerView
{
    FirstPerson,
    Iso
}

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Transform startCheckerPoint;
    [SerializeField] Transform socket;
    [SerializeField] PlayerView view;
    [SerializeField] float checkInterval = .1f;
    bool ShowHide => view == PlayerView.FirstPerson;
    [ShowIf("ShowHide")] public float firstPersonDistance;
    [HideIf("ShowHide")] public float isoRadius;
    GameObject interactableObj;
    [SerializeField] Inventory _inventory;

    #region UnityFunctions
    private void Awake()
    {
        StartCoroutine("CheckingForInteraction");
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            PickUpFromCarrello();
        }
    }
    #endregion
    #region Functions
    private IEnumerator CheckingForInteraction()
    {
        while (true)
        {
            switch (view)
            {
                case PlayerView.FirstPerson:
                    CheckFirstPerson();
                    break;
                case PlayerView.Iso:
                    CheckIsoPerson();
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }
    private void CheckFirstPerson()
    {
        Camera camera = Camera.main;

        if (Physics.Raycast(startCheckerPoint.position, camera.transform.forward, out RaycastHit hitResult, firstPersonDistance))
        {
            interactableObj = hitResult.collider.gameObject;
        }
        else
        {
            interactableObj = null;
        }
    }
    private void CheckIsoPerson()
    {
        Collider[] colliders = Physics.OverlapSphere(startCheckerPoint.position, isoRadius);
        if (colliders.Length != 0)
        {
            float maxDist = int.MaxValue;
            foreach (Collider cl in colliders)
            {
                if (cl.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    float dist = Vector3.Distance(transform.position, cl.transform.position);
                    if (dist < maxDist)
                    {
                        interactableObj = cl.gameObject;
                        maxDist = dist;
                    }
                }
            }
        }
        else
        {
            interactableObj = null;
        }
    }
    private void Interact()
    {
        if (interactableObj != null)
        {
            IInteract _interface = interactableObj.GetComponent<IInteract>();
            if (_interface != null)
            {
                InteractType objType = _interface.GetInteractType();
                switch (objType)
                {
                    case InteractType.Carrello:
                        if (_inventory.GetLastItem)
                        {
                            _interface.Interact(_inventory.GetLastItem);
                        }
                        break;
                    case InteractType.Valigia:
                        if (interactableObj.transform.parent == null)
                        {
                            _interface.Interact();
                            _inventory.AddObjInInventory(interactableObj.transform);
                        }
                        break;
                    case InteractType.Mulino:
                        _interface.Interact(transform);
                        break;
                    case InteractType.Shop:
                        _interface.Interact();
                        break;
                    default:
                        break;
                }
            }

            interactableObj = null;
        }
    }

    private void PickUpFromCarrello()
    {
        Collider[] colliders = Physics.OverlapSphere(startCheckerPoint.position, isoRadius);
        foreach (Collider cl in colliders)
        {
            if (cl.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                Inventory carrelloInventory = cl.GetComponent<Inventory>();
                if (carrelloInventory)
                {
                    Transform obj = carrelloInventory.GetLastItem;
                    if (obj != null)
                    {
                        _inventory.AddObjInInventory(carrelloInventory.GetLastItem);
                    }
                    return;
                }
            }
        }
    }
    #endregion
}
