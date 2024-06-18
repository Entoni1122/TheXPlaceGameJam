using UnityEngine;
using UnityEngine.Splines;

public class TriggerAudioWhenInPosition : MonoBehaviour
{
    float xPosition;
    [SerializeField] AudioClip clip;
    bool playedSound;

    private void Start()
    {
        xPosition = transform.position.x;
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
}
