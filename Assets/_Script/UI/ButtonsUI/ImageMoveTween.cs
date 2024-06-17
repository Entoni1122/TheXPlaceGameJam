using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMoveTween : MonoBehaviour
{
    [SerializeField] Vector3 sizeToGo;
    [SerializeField] float placeToGo;
    [SerializeField] float timeToGoSize;
    [SerializeField] LeanTweenType movemntCurve;
    [SerializeField] LeanTweenType scaleCurve;

    private void Start()
    {
        GetComponent<RectTransform>().LeanMoveLocalY(placeToGo, timeToGoSize).setEase(movemntCurve);
        GetComponent<RectTransform>().LeanScale(sizeToGo, timeToGoSize).setEase(scaleCurve);

        Destroy(gameObject,3f);
    }
}
