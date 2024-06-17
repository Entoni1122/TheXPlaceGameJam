using UnityEngine;
using NaughtyAttributes;
using System.Data;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] EntityType StorableLock;
    [SerializeField] bool isPlayerInventory;
    [SerializeField] public Transform socketRef;
    [SerializeField] Vector3 socketOffsetBaggage;
    [SerializeField] Vector3 socketOffsetPeople;
    [SerializeField] public int maxPickableObj;
    public int count => items.Count;
    public Transform GetLastItem => count > 0 ? items[count - 1] : null;
    public bool IsEmpty => count <= 0;


    private void Awake()
    {
        if (isPlayerInventory)
        {
            PlayerStats.OnChangeStats += (float inSpeedMultiplier, float InForce, bool InMagnetism) =>
            {
                maxPickableObj = (int)InForce;
            };
        }
        GameManager.OnLoseRound += () => items.Clear();
    }

    EntityType currentTypeStored;
    public void AddObjInInventory(Transform obj)
    {
        if (StorableLock != EntityType.NONE)
        {
            if (StorableLock != obj.GetComponent<EntityProp>().entityType) return;
        }

        if (count < maxPickableObj)
        {
            if (count == 0)
            {
                currentTypeStored = obj.GetComponent<EntityProp>().entityType;
            }
            else
            {
                if (currentTypeStored != obj.GetComponent<EntityProp>().entityType) return;
            }
            HandleJointOnAdd(obj);
        }
    }

    private List<Transform> items = new List<Transform>();
    private void HandleJointOnAdd(Transform obj)
    {
        if (items.Count == 0)
        {
            obj.SetParent(socketRef);
            Vector3 offset = currentTypeStored == EntityType.Baggage ? socketOffsetBaggage : socketOffsetPeople;
            obj.position = socketRef.position + offset;
            obj.rotation = socketRef.rotation;
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            Vector3 offset = currentTypeStored == EntityType.Baggage ? socketOffsetBaggage : socketOffsetPeople;
            obj.position = items[count - 1].position + offset;
            obj.rotation = items[count - 1].rotation;
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            ConfigurableJoint joint = items[count - 1].gameObject.AddComponent<ConfigurableJoint>();
            joint.connectedBody = obj.GetComponent<Rigidbody>();
            joint.anchor = new Vector3(0, 0.62f, 0);
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            SoftJointLimit linearLimit = new SoftJointLimit();
            linearLimit.limit = 0.01f;
            joint.linearLimit = linearLimit;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Limited;
            SoftJointLimit angularLimit = new SoftJointLimit();
            angularLimit.limit = 3;
            joint.angularYLimit = angularLimit;
            joint.angularZLimit = angularLimit;
            joint.enableCollision = true;
            joint.projectionMode = JointProjectionMode.PositionAndRotation;
        }
        items.Add(obj);
        obj.GetComponent<EntityProp>().UpdateInventoryRef(this);
    }
    public void HandleOnLostLastItem()
    {
        if (count > 1)
        {
            Destroy(items[count - 2].GetComponent<ConfigurableJoint>());
        }
    }
    public void RemoveLastItem()
    {
        //items[count - 1].GetComponent<EntityProp>().UpdateInventoryRef(null);
        items.RemoveAt(count - 1);
    }
    public void RemoveItem(Transform item)
    {
        for (int i = 0; i < count; i++)
        {
            if (items[i] == item)
            {
                if (i > 1)
                {
                    Destroy(items[i - 1].GetComponent<ConfigurableJoint>());
                }
                Destroy(items[i].GetComponent<ConfigurableJoint>());
                items[i].GetComponent<EntityProp>().UpdateInventoryRef(null);
                items.RemoveAt(i);
                break;
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].GetComponent<ConfigurableJoint>());

            if (i == 0)
            {
                items[i].SetParent(socketRef);
                Vector3 offset = currentTypeStored == EntityType.Baggage ? socketOffsetBaggage : socketOffsetPeople;
                items[i].position = socketRef.position + offset;
                items[i].rotation = socketRef.rotation;
                items[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                Vector3 offset = currentTypeStored == EntityType.Baggage ? socketOffsetBaggage : socketOffsetPeople;
                items[i].position = items[count - 1].position + offset;
                items[i].rotation = items[count - 1].rotation;
                items[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                ConfigurableJoint joint = items[i - 1].gameObject.AddComponent<ConfigurableJoint>();
                joint.connectedBody = items[i].GetComponent<Rigidbody>();
                joint.anchor = new Vector3(0, 0.62f, 0);
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;
                SoftJointLimit linearLimit = new SoftJointLimit();
                linearLimit.limit = 0.01f;
                joint.linearLimit = linearLimit;
                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Limited;
                joint.angularZMotion = ConfigurableJointMotion.Limited;
                SoftJointLimit angularLimit = new SoftJointLimit();
                angularLimit.limit = 3;
                joint.angularYLimit = angularLimit;
                joint.angularZLimit = angularLimit;
                joint.enableCollision = true;
                joint.projectionMode = JointProjectionMode.PositionAndRotation;
            }
        }
    }
    public void ThrowAwayAllItems()
    {
        for (int i = count - 1; i >= 0; i--)
        {
            Transform item = items[i];
            item.parent = null;
            Rigidbody itemRB = item.GetComponent<Rigidbody>();
            itemRB.constraints = RigidbodyConstraints.None;
            itemRB.AddForce(Vector3.up * 10, ForceMode.Impulse);
            Destroy(items[i].GetComponent<ConfigurableJoint>());
        }
        items.Clear();
    }
    public bool IsAlreadyAdded(Transform item)
    {
        foreach (Transform child in items)
        { 
            if (child == item)
            {
                return true;
            }
        }
        return false;   
    }
}
