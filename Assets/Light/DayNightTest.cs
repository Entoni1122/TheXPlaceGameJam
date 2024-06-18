using System.Collections.Generic;
using UnityEngine;

public class DayNightTest : MonoBehaviour
{
    [Header("Day Variables")]
    [Range(0f, 1f)] public float time;
    public float dayLength;
    public float startDay;

    [Header("Sun")]
    [SerializeField] Light sun;
    [SerializeField] AnimationCurve sunIntensity;
    public Gradient sunGradient;

    [Header("Ambient Light")]
    [SerializeField] Light ambient;
    [SerializeField] AnimationCurve ambientIntensity;
    public Gradient ambientGradient;

    [Header("Rendering Settings")]
    [SerializeField] AnimationCurve intensityMultiplier;
    [SerializeField] AnimationCurve reflectionMultiplier;

    [Header("Lamps")]
    [SerializeField] GameObject lamps;
    [Range(0f, 1f)][SerializeField] float turnOff;
    [Range(0f, 1f)][SerializeField] float turnOn;

    public bool timeCycleActivate = true;

    void Start()
    {
        time = startDay / dayLength; // Normalize start time
        SetLighting();
        SetSunAmbientIntensity();
        ToggleLamps();
    }

    void Update()
    {
        if (timeCycleActivate)
        {
            time += Time.deltaTime / dayLength; // Advance time based on day length
            if (time >= 1f)
            {
                time -= 1f; // Reset time at end of day-night cycle
            }
            SetLighting();
            SetSunAmbientIntensity();
            ToggleLamps();
        }
    }

    void SetLighting()
    {
        RenderSettings.ambientIntensity = intensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionMultiplier.Evaluate(time);
    }

    void SetSunAmbientIntensity()
    {
        sun.intensity = sunIntensity.Evaluate(time);
        sun.color = sunGradient.Evaluate(time);

        ambient.intensity = ambientIntensity.Evaluate(time);
        ambient.color = ambientGradient.Evaluate(time);
    }

    void ToggleLamps()
    {
        if ((time >= turnOn || time < turnOff) && !lamps.activeSelf)
        {
            lamps.SetActive(true);
        }
        else if (time > turnOff && time < turnOn && lamps.activeSelf)
        {
            lamps.SetActive(false);
        }
    }
}