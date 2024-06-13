using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Dictionary<string, float> variableValues = new Dictionary<string, float>();

    public float Speed = 1;
    public float Force = 1;
    public float Money = 0;
    // Start is called before the first frame update
    void Start()
    {
        variableValues["Force"] = 1;
        variableValues["Speed"] = 1;
    }

    public void IncreseStats(string StatName)
    {
        variableValues[StatName] += 1;
        if (StatName == "Speed")
        {
            Speed = variableValues[StatName];
        }
        if (StatName == "Force")
        {
            Force = variableValues[StatName];
        }
    }
}
