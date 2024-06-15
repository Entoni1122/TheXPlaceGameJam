using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTweenOnHover : MonoBehaviour
{
    [SerializeField] Vector3 scaleToGo;
    [SerializeField] Vector3 scaleToBack;
    [SerializeField] float timeToGoSize;
    [SerializeField] LeanTweenType curve;
    private void Start()
    {
        LeanTween.scale(transform.gameObject, scaleToGo, timeToGoSize).setEase(curve);
    }
    private void OnEnable()
    {
        LeanTween.scale(transform.gameObject, scaleToGo, timeToGoSize).setEase(curve);
    }
    private void OnDisable()
    {
        LeanTween.scale(transform.gameObject, scaleToBack, timeToGoSize).setEase(curve);
    }
}
