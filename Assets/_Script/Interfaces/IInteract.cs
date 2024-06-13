using UnityEngine;

public interface IInteract
{
    void Interact(Transform socket);
    void Interact(Transform socket,Vector3 offset, int length);
    void ThrowAway(Vector3 impluseForce);
}
