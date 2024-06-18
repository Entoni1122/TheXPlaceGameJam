using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private static float volume = 1f;
    public float AudioManagerVolume => volume;
    private static Transform selfPos;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
        selfPos = transform;
    }

    public void OnVolumeChange(Slider volumeSlider)
    {
        volume = volumeSlider.value;
    }


    public static void PlaySound2d(AudioClip clipAudio,float volumeMultiplier = 1)
    {
        if (clipAudio == null)
        {
            print("Not clip");
            return;

        }
        AudioSource.PlayClipAtPoint(clipAudio, selfPos.position, volume * volumeMultiplier);
    }

    public void UIsound(AudioClip clipAudio)
    {
        if (clipAudio == null)
        {
            print("Not clip");
            return;
        }
        audioSource.PlayOneShot(clipAudio, volume);
    }
}
