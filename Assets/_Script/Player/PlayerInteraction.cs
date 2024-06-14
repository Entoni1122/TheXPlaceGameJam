using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

enum PlayerView
{
    FirstPerson,
    Iso
}

[RequireComponent(typeof(TrajectoryPredictor))]
public class PlayerInteraction : MonoBehaviour
{
    TrajectoryPredictor trajcetory;
    [SerializeField] Transform startCheckerPoint;
    [SerializeField] Transform socket;
    [SerializeField] PlayerView view;
    [SerializeField] float checkInterval = .1f;
    bool ShowHide => view == PlayerView.FirstPerson;
    [ShowIf("ShowHide")] public float firstPersonDistance;
    [HideIf("ShowHide")] public float isoRadius;
    GameObject interactableObj;
    [SerializeField] float throwForce = 10f;
    [SerializeField] float minForce = 2f;
    [SerializeField] float upForceMultiplier = .2f;
    [SerializeField] float forceIncrementMultiplier = 2f;
    float currentForce;

    List<GameObject> objsInSocket = new List<GameObject>();
    private int maxPickableObj;

    #region UnityFunctions
    private void Awake()
    {
        trajcetory = GetComponent<TrajectoryPredictor>();
        StartCoroutine("CheckingForInteraction");

        PlayerStats.OnChangeStats += (float inSpeedMultiplier, float InForce) =>
        {
            maxPickableObj = (int)InForce;
        };
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (objsInSocket.Count > 0)
            {
                trajcetory.SetTrajectoryVisible(true);
                currentForce = minForce;
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (objsInSocket.Count > 0)
            {
                Prediction();
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (objsInSocket.Count > 0)
            {
                Throw();
            }
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
        if (objsInSocket.Count < maxPickableObj)
        {
            if (interactableObj != null)
            {
                IInteract _interface = interactableObj.GetComponent<IInteract>();
                if (_interface != null)
                {
                    bool result = false;

                    if (objsInSocket.Count > 0)
                    {
                        result = _interface.Interact(socket, objsInSocket.Count * socket.up, objsInSocket.Count + 1);
                    }
                    else
                    {
                        result = _interface.Interact(socket);
                    }
                    if (result)
                    {
                        objsInSocket.Add(interactableObj);
                    }

                    interactableObj = null;
                }
            }
        }
    }
    private void Prediction()
    {
        currentForce += Time.deltaTime * forceIncrementMultiplier;
        currentForce = Mathf.Clamp(currentForce, 0, throwForce);
        int lastObjIndex = objsInSocket.Count - 1;
        Rigidbody rb = objsInSocket[lastObjIndex].GetComponent<Rigidbody>();
        ProjectileProperties property = new ProjectileProperties();
        property.Drag = rb.drag;
        property.Mass = rb.mass;

        Vector3 dir = view == PlayerView.FirstPerson
            ? Camera.main.transform.forward + transform.up * upForceMultiplier
            : transform.forward + transform.up * upForceMultiplier;

        property.Direction = dir;
        property.InitialPosition = objsInSocket[lastObjIndex].transform.position;
        property.InitialSpeed = currentForce;
        trajcetory.PredictTrajectory(property);

        if (view == PlayerView.Iso)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit result, 100);
            Vector3 dist = result.point - transform.position;
            dist.Normalize();

            var rot = Quaternion.LookRotation(dist, Vector3.up);
            rot.eulerAngles = Vector3.up * rot.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5 * Time.deltaTime);
        }
    }
    private void Throw()
    {
        int lastObjIndex = objsInSocket.Count - 1;
        IInteract _interface = objsInSocket[lastObjIndex].GetComponent<IInteract>();

        Vector3 dir = view == PlayerView.FirstPerson
            ? Camera.main.transform.forward + transform.up * upForceMultiplier
            : transform.forward + transform.up * upForceMultiplier;

        _interface.ThrowAway(dir * currentForce);
        objsInSocket.RemoveAt(lastObjIndex);
        trajcetory.SetTrajectoryVisible(false);
    }
    #endregion
}
