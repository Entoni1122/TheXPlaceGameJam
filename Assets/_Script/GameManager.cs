using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action OnRoundStart;
    public static Action OnLoseRound;
    [SerializeField] int roundCount;
    [SerializeField] int numberOfValigieToSpawn;
    [SerializeField] int numberOfClientsToSpawn;
    [SerializeField] float TimerOnUI;

    bool startRound;

    private void Start()
    {
        startRound = true;
    }

    private void Update()
    {
        if (startRound)
        {
            startRound = false;
            
            StartingRound();
        }

    }

    void StartingRound()
    {
        OnRoundStart?.Invoke();
    }
}
