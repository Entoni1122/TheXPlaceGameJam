using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class MulinoController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rb;

    [Header("Car Setup")]
    [SerializeField] float forwardAccel = 8f;
    [SerializeField] float reverseAccel = 4f;
    [SerializeField] float turnStrength = 180;
    [SerializeField] float carControlInputOnGround = 3.5f;

    [Header("Ground Check Info")]
    [SerializeField] Transform groundRayPoint;

    private float speedInput, turnInput, verticalInput;
    [SerializeField] bool bEnableController = false;
    public bool BEnableController { get { return bEnableController; } set { bEnableController = value; } }

    [SerializeField] float respawnTimer = 2f;
    bool isGrounded;
    [SerializeField] Transform playerDismountPos;

    [Header("Carrelli")]
    [SerializeField] Transform[] carrelliPosition;
    [SerializeField] GameObject carreloPrefab;
    [SerializeField] List<GameObject> carreloReference;

    private void Update()
    {
        if (bEnableController)
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 2f);
            InputReader();

        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            UnrealPlayerController controller = LibraryFunction.GetUnrealPlayerController();
            controller.EnableInput();
            controller.SetTransfrom(playerDismountPos);
            controller.GetComponent<PlayerAnimation>().NotifyOnCar(false);


            gameObject.layer = LayerMask.NameToLayer("Interactable");
            enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            AttachCarrello();
        }
    }

    private void FixedUpdate()
    {
        if (bEnableController)
        {
            if (!isGrounded)
            {
                return;
            }
            Move();
        }
    }
    private void InputReader()
    {
        float localVerticalInput = Input.GetAxisRaw("Vertical");
        float localTurnInput = Input.GetAxisRaw("Horizontal");

        verticalInput = localVerticalInput != 0 ? localVerticalInput : Mathf.Lerp(verticalInput, 0, Time.deltaTime * carControlInputOnGround);
        turnInput = localTurnInput != 0 ? localTurnInput : Mathf.Lerp(turnInput, 0, Time.deltaTime * carControlInputOnGround);

        OnVerticalInput();
        OnTurnInput();
    }
    private void OnVerticalInput()
    {
        speedInput = 0f;

        if (verticalInput != 0)
        {
            speedInput = verticalInput * (speedInput > 0 ? forwardAccel : reverseAccel);
        }
    }
    private void OnTurnInput()
    {
        if (isGrounded)
        {
            transform.Rotate(0, turnInput * turnStrength * Time.deltaTime, 0, Space.Self);
            return;
        }
    }
    private void Move()
    {
        if (Mathf.Abs(speedInput) > 0)
        {
            rb.AddForce(transform.forward * speedInput, ForceMode.Acceleration);
        }
    }

    int carrelliIndex = 0;
    int maxCarrelli = 3;
    public void AttachCarrello()
    {
        if (carrelliIndex <= 0)
        {
            GameObject carrello = Instantiate(carreloPrefab, carrelliPosition[carrelliIndex].position, Quaternion.identity);
            ConfigurableJoint firstJoint = transform.AddComponent<ConfigurableJoint>();
            SetConfigurableTrain(firstJoint, carrello.GetComponent<Rigidbody>(), new Vector3(0, -1.17f, -3.4f), new Vector3(0, 0, 0.64f));
            carreloReference.Add(carrello);
            carrelliIndex++;
            return;
        }
        else if (carrelliIndex >= maxCarrelli)
        {
            return;
        }
        GameObject otherCarrello = Instantiate(carreloPrefab, carrelliPosition[carrelliIndex].position, Quaternion.identity);
        ConfigurableJoint secondJoint = carreloReference[carrelliIndex - 1].AddComponent<ConfigurableJoint>();
        secondJoint.connectedBody = otherCarrello.GetComponent<Rigidbody>();
        SetConfigurableTrain(secondJoint, otherCarrello.GetComponent<Rigidbody>(), new Vector3(0, 0, -0.74f), new Vector3(0, 0, 0.6f));
        carreloReference.Add(otherCarrello);
        carrelliIndex++;
    }


    void SetConfigurableTrain(ConfigurableJoint joint, Rigidbody rb, Vector3 anchor, Vector3 connectedAnchor)
    {
        //For carrelli anchor vectro3(0,0,-0.74f)    Connectecanchor vector3(0,0,0.6f)
        joint.connectedBody = rb;
        joint.anchor = anchor;
        joint.connectedAnchor = connectedAnchor;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Limited;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
        joint.angularYLimit = new SoftJointLimit() { limit = 80f, bounciness = 0, contactDistance = 0 };
        joint.linearLimit = new SoftJointLimit() { limit = 1, bounciness = 0, contactDistance = 0 };
    }
}
