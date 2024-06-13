using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action OnRoundStart;
    public static Action OnLoseRound;
    [SerializeField] int roundCount;
    public int RoundCount => roundCount;
    [SerializeField] float TimerOnUI;

    bool startRound;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

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
