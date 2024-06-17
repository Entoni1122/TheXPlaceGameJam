using System.Collections;
using UnityEngine;
using NaughtyAttributes;

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
    private Carrello _currentCarrello;
    private bool magnetismON;

    #region UnityFunctions
    private void Awake()
    {
        StartCoroutine("CheckingForInteraction");
        PlayerStats.OnChangeStats += (float speed,float force, bool InMagnetism) =>
        {
            magnetismON = InMagnetism;
        };
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

        if (Input.GetKeyUp(KeyCode.C))
        {
            HandleCarrello();
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
        bool foundInteractable = false;
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
                        foundInteractable = true;
                    }
                }
            }
        }
        if (!foundInteractable)
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
                            _inventory.HandleOnLostLastItem();
                            _interface.Interact(_inventory.GetLastItem);
                            _inventory.RemoveLastItem();

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
                        if (_inventory.IsEmpty)
                        {
                            _interface.Interact(transform);
                        }
                        else
                        {
                            _inventory.HandleOnLostLastItem();
                            _interface.Interact(_inventory.GetLastItem,true);
                            _inventory.RemoveLastItem();
                        }
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
                        carrelloInventory.HandleOnLostLastItem();
                        _inventory.AddObjInInventory(carrelloInventory.GetLastItem);
                        carrelloInventory.RemoveLastItem();
                    }
                    return;
                }
            }
        }
    }

    private void HandleCarrello()
    {
        if (_currentCarrello != null)
        {
            _currentCarrello.Release(transform);
            _currentCarrello = null;
            GetComponent<PlayerAnimation>().NotifyOnCart(false);
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(startCheckerPoint.position, isoRadius);
            foreach (Collider cl in colliders)
            {
                Carrello carrello = cl.GetComponent<Carrello>();
                if (carrello != null)
                {
                    if (cl.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                    {
                        carrello.Handle(transform);
                        _currentCarrello = carrello;
                        _currentCarrello.magnetActive = magnetismON;
                        GetComponent<PlayerAnimation>().NotifyOnCart(true);
                        return;
                    }
                }
            }
        }
    }
    #endregion
}
