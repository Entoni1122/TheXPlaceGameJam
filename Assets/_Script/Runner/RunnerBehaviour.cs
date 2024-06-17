using TreeEditor;
using UnityEngine;

public class RunnerBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
    private Transform targetPoint;
    private Rigidbody _rb;
    int lastIndex = -1;
    [SerializeField] GameObject OwnerRef;

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
    }
    private void SetRandomPoint()
    {
        if (lastIndex >= 0)
        {
            int currentIndex = lastIndex;
            while (currentIndex == lastIndex)
            {
                currentIndex = Random.Range(0, RunnerPath.GetPathCount());
            }
            lastIndex = currentIndex;
        }
        else
        {
            lastIndex = Random.Range(0, RunnerPath.GetPathCount());
        }
        targetPoint = RunnerPath.GetPathAtIndex(lastIndex);
    }

    private void Update()
    {
        if (targetPoint != null)
        {
            Vector3 dir = targetPoint.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            _rb.AddForce(dir * speed * Time.deltaTime, ForceMode.Force);
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            float dist = Mathf.Abs((targetPoint.position - transform.position).magnitude);
            if (dist < 5)
            {
                SetRandomPoint();
            }
        }
    }
    public Transform InteractWithRunenrBaggage()
    {
        OwnerRef.SetActive(true);
        return OwnerRef.transform;
    }
}