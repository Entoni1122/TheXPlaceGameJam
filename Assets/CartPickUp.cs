using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartPickUp : MonoBehaviour
{
    [SerializeField] Transform targetPOs;
    [SerializeField] List<GameObject> objsInSocket;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            IInteract _interface = other.gameObject.GetComponent<IInteract>();
            if (_interface != null)
            {
                if (objsInSocket.Count > 0)
                {
                    _interface.Interact(targetPOs, objsInSocket.Count * Vector3.up, objsInSocket.Count + 1);
                }
                else
                {
                    _interface.Interact(targetPOs);
                }
                objsInSocket.Add(other.gameObject);
            }
        }
    }
}
