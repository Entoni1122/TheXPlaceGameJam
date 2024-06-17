using UnityEngine;

public class FollowTargetForCollision : MonoBehaviour
{
    [SerializeField] GameObject colliderType;
    [SerializeField] GameObject objref;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform target;
    [SerializeField] Vector3 center;
    [SerializeField] Vector3 size;
    BoxCollider bc;

    private void Awake()
    {
        GameObject ff = Instantiate(colliderType);
        objref = ff;
        objref.transform.parent = null;
        bc = ff.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        bc.center = center;
        bc.size = size;
    }
    private void Update()
    {
        objref.transform.position = target.position;
        objref.transform.eulerAngles = target.eulerAngles;
    }
}
