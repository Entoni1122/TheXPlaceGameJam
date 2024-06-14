using System;
using UnityEngine;



public enum EntityType
{
    People,
    Baggage
}

public class EntityProp : MonoBehaviour
{
    public EntityType entityType { get; private set; }
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;
    private Transform target;
    private Vector3 dir;
    public int spotID { get;private set; }
    private Action Move;


    public void Init(Transform inTarget, EntityType _type, int targetSpotID = 0)
    {
        transform.parent = null;
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        target = inTarget;
        dir = target.position - transform.position;
        dir.Normalize();
        spotID = targetSpotID;
        Move = StartMovement;
        entityType = _type;
    }

    public void GoToStorage(Transform inTarget)
    {
        transform.parent = null;
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        target = inTarget;
        dir = target.position - transform.position;
        dir.Normalize();
        Move = StartMovement;
        speed *= 2f;
    }



    void Update()
    {
        Move?.Invoke();
    }
    private void StartMovement()
    {
        if (target != null)
        {
            if (Vector3.Distance(target.position, transform.position) < 1f)
            {
                rb.useGravity = true;
                transform.parent = null;
                Move = null;
            }
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(dir * speed * Time.deltaTime, ForceMode.Acceleration);
    }
}
