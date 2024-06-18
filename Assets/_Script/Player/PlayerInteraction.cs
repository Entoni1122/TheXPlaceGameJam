using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

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
    public bool magnetismON;
    private GameObject lastInteractableObj;
    #region UnityFunctions
    private void Awake()
    {
        StartCoroutine("CheckingForInteraction");
        PlayerStats.OnChangeStats += (float speed, float force, bool InMagnetism) =>
        {
            magnetismON = InMagnetism;
        };
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Interact();
            HandleCarrello();
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

    private void ToggleOutline(GameObject obj, bool state)
    {
        var outline = obj.GetComponent<BlinkOutline>();
        if (outline != null)
        {
            if (state)
            {
                outline.OutlineWidth = 3f;
                outline.OutlineColor = Color.white;
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning($"No BlinkOutline component found on {obj.name}");
        }
    }

    private void CheckIsoPerson()
    {
        bool foundInteractable = false;
        Collider[] colliders = Physics.OverlapSphere(startCheckerPoint.position, isoRadius);
        if (colliders.Length != 0)
        {
            float maxDist = float.MaxValue;
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

        HandleOutline();
    }

    private void HandleOutline()
    {
        if (interactableObj != null)
        {
            if (lastInteractableObj != null && lastInteractableObj != interactableObj)
            {
                ToggleOutline(lastInteractableObj, false);
            }
            ToggleOutline(interactableObj, true);
            lastInteractableObj = interactableObj;
        }
        else
        {
            if (lastInteractableObj != null)
            {
                ToggleOutline(lastInteractableObj, false);
                lastInteractableObj = null;
            }
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
                            _interface.Interact(_inventory.GetLastItem, true);
                            _inventory.RemoveLastItem();
                        }
                        break;
                    case InteractType.Shop:
                        _interface.Interact();
                        break;
                    default:
                        break;
                }
                return;
            }

            if (!_inventory.IsFull)
            {
                RunnerBehaviour runner = interactableObj.GetComponent<RunnerBehaviour>();
                if (runner)
                {
                    _inventory.AddObjInInventory(runner.InteractWithRunenrBaggage());
                    Destroy(interactableObj);
                }
                interactableObj = null;
            }
        }
    }

    private void PickUpFromCarrello()
    {
        if (!_inventory.IsFull)
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
            if (_inventory.IsEmpty)
            {
                Collider[] colliders = Physics.OverlapSphere(startCheckerPoint.position, isoRadius);
                foreach (Collider cl in colliders)
                {
                    if (cl.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                    {
                        Carrello carrello = cl.GetComponent<Carrello>();
                        if (carrello != null)
                        {
                            if (!carrello.CanBeHandle) return;
                            if (carrello.Handle(transform))
                            {
                                _currentCarrello = carrello;
                                _currentCarrello.magnetActive = magnetismON;
                                GetComponent<PlayerAnimation>().NotifyOnCart(true);
                            }
                            return;
                        }
                    }
                }
            }
        }
    }
    #endregion
}
