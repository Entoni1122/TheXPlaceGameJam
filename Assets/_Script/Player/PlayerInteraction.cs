using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

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
    [SerializeField] float checkInterval = 0.1f;
    bool ShowHide => view == PlayerView.FirstPerson;
    [ShowIf("ShowHide")] public float firstPersonDistance;
    [HideIf("ShowHide")] public float isoRadius;
    GameObject interactableObj;
    [SerializeField] float throwForce = 10;

    List<GameObject> objsInSocket = new List<GameObject>();

    #region UnityFunctions
    private void Start()
    {
        StartCoroutine("CheckingForInteraction");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (objsInSocket.Count <= 0) return;


            int lastObjIndex = objsInSocket.Count - 1;
            IInteract _interface = objsInSocket[lastObjIndex].GetComponent<IInteract>();
            _interface.ThrowAway((transform.forward + transform.up) * throwForce);
            objsInSocket.RemoveAt(lastObjIndex); 
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
        if (Physics.SphereCast(startCheckerPoint.position, isoRadius, Vector3.one, out RaycastHit hitResult))
        {
            interactableObj = hitResult.collider.gameObject;
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
            if (objsInSocket.Count > 0)
            {
                _interface.Interact(socket, objsInSocket.Count * Vector3.up, objsInSocket.Count + 1);
            }
            else
            {
                _interface.Interact(socket);
            }
            objsInSocket.Add(interactableObj);
            interactableObj = null;
        }
    }
    #endregion
}
