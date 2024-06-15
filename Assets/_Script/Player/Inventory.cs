using UnityEngine;
using NaughtyAttributes;

public class Inventory : MonoBehaviour
{
    [SerializeField] EntityType StorableLock;
    [SerializeField] bool isPlayerInventory;
    [SerializeField] public Transform socketRef;
    [SerializeField] Vector3 socketOffsetBaggage;
    [SerializeField] Vector3 socketOffsetPeople;
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

    EntityType currentTypeStored;
    public void AddObjInInventory(Transform obj)
    {
        if (StorableLock != EntityType.NONE)
        {
            if (StorableLock != obj.GetComponent<EntityProp>().entityType) return;
        }

        if (count < maxPickableObj)
        {
            if(count == 0)
            {
                currentTypeStored = obj.GetComponent<EntityProp>().entityType;
            }
            else
            {
                if (currentTypeStored != obj.GetComponent<EntityProp>().entityType)return;
            }
            obj.SetParent(socketRef);
            Vector3 offset = currentTypeStored == EntityType.Baggage ? socketOffsetBaggage : socketOffsetPeople;
            obj.position = socketRef.position + (count - 1) * socketRef.up;
            obj.position += (count - 1) * offset;
            obj.rotation = socketRef.rotation;
        }
    }
}
