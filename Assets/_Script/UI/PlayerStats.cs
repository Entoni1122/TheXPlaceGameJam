using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Dictionary<string, float> variableValues = new Dictionary<string, float>();

    public float Speed = 1;
    public float Force = 1;
    public float Money = 0;

    public static Action<float,float> OnChangeStats;//remember first is Speed, second is Force


    // Start is called before the first frame update
    void Start()
    {
        variableValues["Force"] = Speed;
        variableValues["Speed"] = Force;


        OnChangeStats?.Invoke(Speed, Force);
    }

    public void IncreseStats(string StatName)
    {
        variableValues[StatName] += 1f;
        if (StatName == "Speed")
        {
            Speed = variableValues[StatName] * 0.2f;
        }
        if (StatName == "Force")
        {
            Force = variableValues[StatName];
        }

        OnChangeStats?.Invoke(Speed,Force);
    }
}
