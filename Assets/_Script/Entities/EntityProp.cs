using System;
using UnityEditor.Rendering;
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
    Red,
    Green
}

public class EntityProp : MonoBehaviour
{
    public EntityType entityType { get; private set; }
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;
    private Transform target;
    public ColorType color { get; private set; }
    private Action Move;
    private Action TransitioAnim;
    private Inventory inventory;
    [SerializeField] GameObject runnerPrefab;


    private void Start()
    {
        GameManager.OnLoseRound += OnLoseRound;
    }
    private void OnLoseRound()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        GameManager.OnLoseRound -= OnLoseRound;
    }

    public void Init(Transform inTarget, EntityType _type, ColorType _color)
    {
        if (_type == EntityType.Baggage)
            rb.constraints = RigidbodyConstraints.None;

        target = inTarget;
        color = _color;
        Move = StartMovement;
        entityType = _type;
        gameObject.layer = 0;


        Color meshcolor = Color.white;
        switch (color)
        {
            case ColorType.Blue:
                meshcolor = Color.blue;
                break;
            case ColorType.Red:
                meshcolor = Color.red;
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
    public void UpdateRefFromRunner(ColorType InColor, EntityType _type)
    {
        if (_type == EntityType.Baggage)
            rb.constraints = RigidbodyConstraints.None;

        color = InColor;
        entityType = _type;

        Color meshcolor = Color.white;
        switch (color)
        {
            case ColorType.Blue:
                meshcolor = Color.blue;
                break;
            case ColorType.Red:
                meshcolor = Color.red;
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
        TransitioAnim = TransitionAnimationOnEnable;
    }
    public void UpdateInventoryRef(Inventory InInventory)
    {
        inventory = InInventory;
    }
    public void GoToStorage(Transform inTarget)
    {
        if (inventory)
        {
            inventory.RemoveItem(transform);
        }
        transform.parent = null;
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = false;
        target = inTarget;
        Move = StartMovement;
        speed *= 2f;
        gameObject.layer = 0;
        GameManager.OnLoseRound -= OnLoseRound;
    }
    void Update()
    {
        Move?.Invoke();
        TransitioAnim?.Invoke();
    }
    private void StartMovement()
    {
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            dir.Normalize();
            rb.AddForce(dir * speed * Time.deltaTime, ForceMode.Acceleration);

            if (Vector3.Distance(target.position, transform.position) < 1f)
            {
                transform.parent = null;
                Move = null;
                gameObject.layer = LayerMask.NameToLayer("Interactable");
                Invoke("ChangeStatus", 5);
            }
        }
    }
    private void ChangeStatus()
    {
        if (inventory is null)
        {
            if (runnerPrefab)
            {
                gameObject.layer = 0;
                TransitioAnim = TransitionAnimationOnDisable;
            }
        }
    }
    float timer = .2f;
    private void TransitionAnimationOnDisable()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            gameObject.layer = 0;
            timer = .2f;
            GameObject runner = Instantiate(runnerPrefab, transform.position, Quaternion.identity);
            runner.GetComponent<RunnerBehaviour>().Init(gameObject, color);
            gameObject.SetActive(false);
            TransitioAnim = null;
        }
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / .2f);

    }
    private void TransitionAnimationOnEnable()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = .2f;
            gameObject.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            transform.localScale = Vector3.one;
            TransitioAnim = null;
            return;
        }
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / .2f);
    }

}