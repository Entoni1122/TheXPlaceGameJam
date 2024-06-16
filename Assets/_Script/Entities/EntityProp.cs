using System;
using UnityEngine;


public enum EntityType
{
    People,
    Baggage,
    NONE
}
public enum ColorType
{
    Blue,
    Orange,
    Green
}

public class EntityProp : MonoBehaviour
{
    public EntityType entityType { get; private set; }
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;
    private Transform target;
    private Vector3 dir;
    public ColorType color { get; private set; }
    private Action Move;


    public void Init(Transform inTarget, EntityType _type, ColorType _color)
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        target = inTarget;
        dir = target.position - transform.position;
        dir.Normalize();
        color = _color;
        Move = StartMovement;
        entityType = _type;


        Color meshcolor = Color.white;
        switch (color)
        {
            case ColorType.Blue:
                meshcolor = Color.blue;
                break;
            case ColorType.Orange:
                meshcolor = Color.yellow;
                break;
            case ColorType.Green:
                meshcolor = Color.green;
                break;
            default:
                break;
        }

        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer)
        {
            meshRenderer.materials[1].color = meshcolor;
        }
        else
        {
            SkinnedMeshRenderer skinnedmeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (skinnedmeshRenderer)
            {
                skinnedmeshRenderer.materials[5].color = meshcolor;
            }

        }

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
            if (Vector3.Distance(target.position, transform.position) < 2f)
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
