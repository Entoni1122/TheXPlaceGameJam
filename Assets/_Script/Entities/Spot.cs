using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField] ColorType color;
    public ColorType GetColor => color;
    [SerializeField] Transform target;
    public int amountBaggage { private get; set; }
    private int currentAmountBaggage;
    public int amountPeople { private get; set; }
    private int currentAmountPeople;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EntityProp>().color == color)
        {
            if (other.GetComponent<EntityProp>().entityType == EntityType.Baggage)
            {
                currentAmountBaggage++;
                other.transform.gameObject.layer = 0;
                other.GetComponent<EntityProp>().GoToStorage(target);
                Destroy(other.gameObject, 5);
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
        }
    }
}
