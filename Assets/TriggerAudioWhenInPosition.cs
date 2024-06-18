using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerAudioWhenInPosition : MonoBehaviour
{
    [SerializeField] float xPosition;
    [SerializeField] AudioClip clip;
    private void Update()
    {
        if (transform.position.x == xPosition)
        {
            AudioManager.PlaySound2d(clip);
        }
    }
}
