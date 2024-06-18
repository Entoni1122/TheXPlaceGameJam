using UnityEngine;

public enum InteractType
{
    Carrello,
    Valigia,
    Shop,
    Mulino,
    People
}
public interface IInteract
{
    InteractType GetInteractType();
    void Interact();
    void Interact(Transform baggage);
    void Interact(Transform baggage,bool bHasToStore);
}
