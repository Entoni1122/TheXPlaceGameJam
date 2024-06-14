using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] bool isPlayerInventory;
    [SerializeField] Transform socketRef;
    List<Transform> objsInSocket = new List<Transform>();
    [SerializeField] int maxPickableObj;

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

    public Transform GetLastItem => objsInSocket.Count > 0 ? objsInSocket[objsInSocket.Count - 1] : null;
    public bool IsEmpty => objsInSocket.Count <= 0;

    public void AddObjInInventory(Transform obj)
    {
        if (objsInSocket.Count < maxPickableObj)
        {
            objsInSocket.Add(obj);
            obj.SetParent(socketRef);
            obj.position = socketRef.position + objsInSocket.Count * socketRef.up;
            obj.rotation = socketRef.rotation;
        }
    }

    public void Remove()
    {
        objsInSocket.RemoveAt(objsInSocket.Count - 1);
    }
}
