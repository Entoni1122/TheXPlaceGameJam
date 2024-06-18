using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Shooter : MonoBehaviour
{
    TrajectoryPredictor trajcetory;
    Inventory _inventory;
    [SerializeField] PlayerView view;
    [SerializeField] float throwForce = 10f;
    [SerializeField] float minForce = 2f;
    [SerializeField] float upForceMultiplier = .2f;
    [SerializeField] float forceIncrementMultiplier = 2f;
    [SerializeField] AudioClip sound;
    float currentForce;
    private bool canShoot;

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
        trajcetory = GetComponent<TrajectoryPredictor>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!_inventory.IsEmpty)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                trajcetory.SetTrajectoryVisible(true);
                currentForce = minForce;
                canShoot = true;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Prediction();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Throw();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            trajcetory.SetTrajectoryVisible(false);
            canShoot = false;
        }
    }

    private void Prediction()
    {
        currentForce += Time.deltaTime * forceIncrementMultiplier;
        currentForce = Mathf.Clamp(currentForce, 0, throwForce);
        Rigidbody rb = _inventory.GetLastItem.GetComponent<Rigidbody>();
        ProjectileProperties property = new ProjectileProperties();
        property.Drag = rb.drag;
        property.Mass = rb.mass;

        Vector3 dir = view == PlayerView.FirstPerson
            ? Camera.main.transform.forward + transform.up * upForceMultiplier
            : transform.forward + transform.up * upForceMultiplier;

        property.Direction = dir;
        property.InitialPosition = _inventory.GetLastItem.transform.position;
        property.InitialSpeed = currentForce;
        trajcetory.PredictTrajectory(property);

        if (view == PlayerView.Iso)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit result, 100);
            Vector3 dist = result.point - transform.position;
            dist.Normalize();

            var rot = Quaternion.LookRotation(dist, Vector3.up);
            rot.eulerAngles = Vector3.up * rot.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5 * Time.deltaTime);
        }
    }
    private void Throw()
    {
        if (!canShoot) return;
        IInteract _interface = _inventory.GetLastItem.GetComponent<IInteract>();

        Vector3 dir = view == PlayerView.FirstPerson
            ? Camera.main.transform.forward + transform.up * upForceMultiplier
            : transform.forward + transform.up * upForceMultiplier;

        _inventory.HandleOnLostLastItem();
        _inventory.GetLastItem.GetComponent<BaseInteractableObj>().ThrowAway(dir * currentForce);
        _inventory.RemoveLastItem();
        trajcetory.SetTrajectoryVisible(false);
        AudioManager.PlaySound2d(sound);
    }
}
