using UnityEngine;

public interface IInteract
{
    bool Interact(Transform socket);
    bool Interact(Transform socket,Vector3 offset, int length);
    void ThrowAway(Vector3 impluseForce);
}
