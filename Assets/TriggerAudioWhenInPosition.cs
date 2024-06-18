using UnityEngine;

public class TriggerAudioWhenInPosition : MonoBehaviour
{
    float xPosition;
    [SerializeField] AudioClip clip;

    private void Start()
    {
        xPosition = transform.position.x;
    }
    bool playedSound;
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
            playedSound = false ;
        }
    }
}
