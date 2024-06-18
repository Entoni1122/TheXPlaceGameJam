using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeMulettoLogic : MonoBehaviour
{

    [Header("Carrelli")]
    [SerializeField] Vector2 carrelloOffset;
    [SerializeField] GameObject carreloPrefab;
    [SerializeField] List<GameObject> carreloReference;

    private void OnEnable()
    {
        BuyEquipment.upgrade += AttachCarrello;
    }
    private void OnDisable()
    {
        BuyEquipment.upgrade -= AttachCarrello;
    }

    private void Start()
    {
       AttachCarrello();
    }

    int carrelliIndex = 0;
    int maxCarrelli = 3;
    public void AttachCarrello()
    {
        Vector3 pos = carrelliIndex <= 0 ?
              transform.position + transform.forward * carrelloOffset.x :
              carreloReference[carrelliIndex - 1].transform.position + carreloReference[carrelliIndex - 1].transform.forward * carrelloOffset.x;
        pos.y = carrelloOffset.y;

        Quaternion rot = carrelliIndex <= 0 ?
            transform.rotation :
            carreloReference[carrelliIndex - 1].transform.rotation;

        if (carrelliIndex <= 0)
        {
            GameObject carrello = Instantiate(carreloPrefab, pos, rot);
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
        GameObject otherCarrello = Instantiate(carreloPrefab, pos, rot);
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
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Limited;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
        joint.angularYLimit = new SoftJointLimit() { limit = 80f, bounciness = 0, contactDistance = 0 };
        joint.linearLimit = new SoftJointLimit() { limit = 1, bounciness = 0, contactDistance = 0 };
    }
}
