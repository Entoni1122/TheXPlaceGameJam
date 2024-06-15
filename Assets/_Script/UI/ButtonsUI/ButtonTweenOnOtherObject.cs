using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTweenOnOtherObject : MonoBehaviour
{
    [SerializeField] Vector3 placeToGo;
    [SerializeField] Vector3 placeToGoBack;
    [SerializeField] float timeToTween;
    [SerializeField] LeanTweenType curve;
    [SerializeField] GameObject objectToMove;
    private void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(TweenClick);
    }
    void TweenClick()
    {
        LeanTween.scale(objectToMove, placeToGo, timeToTween).setEase(curve);
    }
}
