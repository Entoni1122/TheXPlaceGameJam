using System;
using UnityEngine;

public class RunnerBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
    private Transform targetPoint;
    private Rigidbody _rb;
    int lastIndex = -1;
    [SerializeField] GameObject OwnerRef;
    private Action TransitioAnim;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        GameManager.OnLoseRound += () => { Destroy(gameObject); };
    }
    public void Init(GameObject Owner,ColorType colorType)
    {
        SetRandomPoint();
        OwnerRef = Owner;
        Color color = Color.white;  
        switch (colorType)
        {
            case ColorType.Blue:
                color = Color.blue;
                break;
            case ColorType.Red:
                color = Color.red;
                break;
            case ColorType.Green:
                color = Color.green;
                break;
            default:
                break;
        }
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().materials[1].color = color;
        TransitioAnim = TransitionAnimationOnEnable;
        gameObject.layer = 0;
    }
    private void SetRandomPoint()
    {
        if (lastIndex >= 0)
        {
            int currentIndex = lastIndex;
            while (currentIndex == lastIndex)
            {
                currentIndex = UnityEngine.Random.Range(0, RunnerPath.GetPathCount());
            }
            lastIndex = currentIndex;
        }
        else
        {
            lastIndex = UnityEngine.Random.Range(0, RunnerPath.GetPathCount());
        }
        targetPoint = RunnerPath.GetPathAtIndex(lastIndex);
    }

    private void Update()
    {
        TransitioAnim?.Invoke();
        if (targetPoint != null)
        {
            Vector3 dir = targetPoint.position - transform.position;
            dir.y = 0;
            dir.Normalize();

            dir = AvoidObstacles(dir);

            _rb.AddForce(dir * speed * Time.deltaTime, ForceMode.Force);
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation , targetRot, 5 * Time.deltaTime);
            float dist = Mathf.Abs((targetPoint.position - transform.position).magnitude);
            if (dist < 5)
            {
                SetRandomPoint();
            }
        }
    }

    private Vector3 AvoidObstacles(Vector3 direction)
    {
        RaycastHit hit;
        float sphereRadius = 2f;
        float distance = 1f;

        if (Physics.SphereCast(transform.position, sphereRadius, direction, out hit, distance))
        {
            if (hit.collider != null)
            {
                return Vector3.Cross(hit.normal, Vector3.up).normalized;
            }
        }
        return direction;
    }
    
    public Transform InteractWithRunenrBaggage()
    {
        OwnerRef.SetActive(true);
        return OwnerRef.transform;
    }

    float timer = .1f;
    private void TransitionAnimationOnEnable()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            gameObject.layer = 0;
            timer = .1f;
            transform.localScale = Vector3.one;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            TransitioAnim = null;
            return;
        }
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / .1f);
    }
}