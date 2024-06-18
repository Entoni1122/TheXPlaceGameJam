using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static float volume = 1f;
    private static Transform selfPos;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        selfPos = transform;
    }

    public void OnVolumeChange(Slider volumeSlider)
    {
        volume = volumeSlider.value;
    }


    public static void PlaySound2d(AudioClip clipAudio)
    {
        if (clipAudio == null) return;
        AudioSource.PlayClipAtPoint(clipAudio, selfPos.position, volume);
    }
}
