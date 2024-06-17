using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Dictionary<string, float> variableValues = new Dictionary<string, float>();

    public float Speed = 1;
    public float Force = 1;

    public static Action<float, float,bool> OnChangeStats;

    private bool magnetismOn;

    void Start()
    {
        variableValues["Force"] = Speed;
        variableValues["Speed"] = Force;

        OnChangeStats?.Invoke(Speed, Force, magnetismOn);
    }

    public void IncreseStats(string StatName)
    {
        if (StatName == "Magnetism")
        {
            magnetismOn = true;
            OnChangeStats?.Invoke(Speed, Force, magnetismOn);
            return;
        }

        variableValues[StatName] += 1f;
        if (StatName == "Speed")
        {
            Speed = variableValues[StatName] * 0.2f;
        }
        if (StatName == "Force")
        {
            Force = variableValues[StatName];
        }

        OnChangeStats?.Invoke(Speed, Force, magnetismOn);
    }
}
