using UnityEngine;


public class BaseInteractableObj : MonoBehaviour, IInteract
{
    private Rigidbody _rb;
    [SerializeField] Vector3 objectOffset = Vector3.zero;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void IInteract.Interact(Transform socket)
    {
        transform.parent = socket;
        transform.position = transform.parent.position + objectOffset;
        _rb.useGravity = false;
    }

    void IInteract.Interact(Transform socket, Vector3 offset, int length)
    {
        transform.parent = socket;
        transform.position = transform.parent.position + offset + objectOffset * length;
        _rb.useGravity = false;
    }

    void IInteract.ThrowAway(Vector3 impluseForce)
    {
        transform.parent = null;
        _rb.AddForce(impluseForce,ForceMode.Impulse);
        _rb.useGravity = true;
    }
}