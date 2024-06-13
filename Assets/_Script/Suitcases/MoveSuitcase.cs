using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSuitcase : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;
    Transform target;
    Vector3 dir;

    public void Init(Transform inTarget)
    {
        rb.useGravity = false;
        target = inTarget;
        dir = target.position - transform.position;
        dir.Normalize();
    }

    void Update()
    {
        if (target != null)
        {
            if (Vector3.Distance(target.position, transform.position) < .5f)
            {
                rb.useGravity = true;
                Destroy(this);
            }
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(dir * speed * Time.deltaTime, ForceMode.Acceleration);
    }
}
