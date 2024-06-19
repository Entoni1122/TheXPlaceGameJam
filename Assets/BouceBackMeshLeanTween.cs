using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouceBackMeshLeanTween : MonoBehaviour
{
    [SerializeField] Vector3 sizeToGo;
    [SerializeField] Vector3 sizeToGoBack;
    [SerializeField] float timeToTween;
    [SerializeField] LeanTweenType curve;
    private void Start()
    {
        transform.LeanScale(sizeToGo, timeToTween).setEase(curve);
    }
    private void Update()
    {
        if (transform.localScale == sizeToGo)
        {
            transform.LeanScale(sizeToGoBack, timeToTween).setEase(curve);

        }
        else if (transform.localScale == sizeToGoBack)
        {
            transform.LeanScale(sizeToGo, timeToTween).setEase(curve);
        }
    }
}
