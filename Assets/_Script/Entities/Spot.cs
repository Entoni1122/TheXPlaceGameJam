using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField] int ID;
    [SerializeField] Transform target;
    public int amountBaggage;
    private int currentAmountBaggage;
    public int amountPeople;
    private int currentAmountPeople;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EntityProp>().spotID == ID)
        {
            if (other.GetComponent<EntityProp>().entityType == EntityType.Baggage)
            {
                currentAmountBaggage++;
                other.transform.gameObject.layer = 0;
                other.GetComponent<EntityProp>().GoToStorage(target);
                Destroy(other.gameObject, 5);
                if (currentAmountBaggage >= amountBaggage) { print("FullBaggage"); }
            }
            else
            {
                currentAmountPeople++;
                if (amountPeople > currentAmountPeople) { print("FullPeople"); }
            }
            print("CorrectSpot");
        }
        else
        {
            print("WrongSpot");
        }
    }
}
