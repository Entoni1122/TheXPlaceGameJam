using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField] ColorType color;
    public ColorType GetColor => color;
    [SerializeField] Transform start;
    [SerializeField] Transform target;
    public int amountBaggage { private get; set; }
    private int currentAmountBaggage;
    public int amountPeople { private get; set; }
    private int currentAmountPeople;

    [SerializeField] int moneyGet = 40;
    public static event Action<int> OnScorePoint;

    private void OnTriggerEnter(Collider other)
    {
        EntityProp entity = other.GetComponent<EntityProp>();

        if (!entity || entity.IsAlreadyAdded) return;


        if (entity.color == color)
        {
            entity.IsAlreadyAdded = true;
            OnScorePoint?.Invoke(moneyGet);
            if (entity.entityType == EntityType.Baggage)
            {
                currentAmountBaggage++;
                other.transform.gameObject.layer = 0;

                if (currentAmountBaggage >= amountBaggage)
                {
                    GameManager.instance.OnFullSpot(amountBaggage, 0);
                    currentAmountBaggage = 0;
                }
            }
            else
            {
                currentAmountPeople++;
                if (amountPeople >= currentAmountPeople)
                {
                    GameManager.instance.OnFullSpot(0, amountPeople);
                    currentAmountPeople = 0;
                }
            }

            other.GetComponent<EntityProp>().GoToStorage(target,start);
            Destroy(other.gameObject, 2);
        }
    }
}
