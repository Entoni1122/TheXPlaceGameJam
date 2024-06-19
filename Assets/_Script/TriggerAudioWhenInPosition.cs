using UnityEngine;
using UnityEngine.Splines;

public class TriggerAudioWhenInPosition : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] float repeatingTimer;
    float xPosition;
    bool playedSound;
    SplineAnimate spline;

    private void Start()
    {
        spline  = GetComponent<SplineAnimate>();
        xPosition = transform.position.x;
        InvokeRepeating("Restart", 0, repeatingTimer);
    }
    private void Update()
    {
        if (Mathf.Abs(transform.position.x - xPosition) <= 1f)
        {
            if (!playedSound)
            {
                AudioManager.PlaySound2d(clip);
                playedSound = true;
            }
        }
        else
        {
            playedSound = false;
        }
    }

    private void Restart()
    {
        spline.Restart(true);
    }
}
