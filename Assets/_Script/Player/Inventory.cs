using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] bool isPlayerInventory;
    [SerializeField] Transform socketRef;
    [SerializeField] Vector3 socketOffset;
    [SerializeField] int maxPickableObj;
    private int count => socketRef.childCount;
    public Transform GetLastItem => count > 0 ? socketRef.GetChild(count - 1) : null;
    public bool IsEmpty => count <= 0;

    private void Awake()
    {
        if (isPlayerInventory)
        {
            PlayerStats.OnChangeStats += (float inSpeedMultiplier, float InForce) =>
            {
                maxPickableObj = (int)InForce;
            };
        }
    }

   

    public void AddObjInInventory(Transform obj)
    {
        if (count < maxPickableObj)
        {
            obj.SetParent(socketRef);
            obj.position = socketRef.position + (count - 1) * socketRef.up;
            obj.position += (count - 1) * socketOffset;
            obj.rotation = socketRef.rotation;
        }
    }
}
