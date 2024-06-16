using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnrealPlayerController : MonoBehaviour
{

    [SerializeField] List<MonoBehaviour> scriptsReference = new List<MonoBehaviour>();
    private void Awake()
    {
        LibraryFunction.SetUnrealPlayerController(this);
    }
    public void DisableInput()
    {
        foreach (var script in scriptsReference)
        {
            script.enabled = false;
        }
        GetComponent<Rigidbody>().isKinematic = true;
    }
    public void EnableInput()
    {
        foreach (var script in scriptsReference)
        {
            script.enabled = true;
        }
        GetComponent<Rigidbody>().isKinematic = false;

    }

    public async void OnDeath()
    {
        foreach (var script in scriptsReference)
        {
            script.enabled = false;
        }

        await Task.Delay(2000);

        EnableInput();
        transform.eulerAngles = Vector3.zero;
    }

    public void SetTransfrom(Transform InTransform)
    {
        transform.position = InTransform.position;
        transform.rotation = InTransform.rotation;
    }
}
