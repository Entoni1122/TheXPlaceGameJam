using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct EntetiesPerLevel
{
    public int level;
    public int count;
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action OnRoundStart;
    public static Action OnLoseRound;
    [SerializeField] int roundCount;
    public int RoundCount => roundCount;
    [SerializeField] float TimerOnUI;

    bool startRound;


    [Header("Gameplay")]
    [SerializeField] List<Spot> spotsList;
    [SerializeField] List<EntetiesPerLevel> baggagesPerRound;
    [SerializeField] List<EntetiesPerLevel> peoplesPerRound;
    private Dictionary<EntityType, Dictionary<int, int>> _dictGame = new Dictionary<EntityType, Dictionary<int, int>>();

    private void Awake()
    {
        instance = this;
        SetupGame();
    }

    private void SetupGame()
    {
        startRound = true;

        _dictGame[EntityType.Baggage] = new Dictionary<int, int>();
        _dictGame[EntityType.People] = new Dictionary<int, int>();


        foreach (EntetiesPerLevel entity in baggagesPerRound)
        {
            _dictGame[EntityType.Baggage][entity.level] = entity.count;
        }
        foreach (EntetiesPerLevel entity in peoplesPerRound)
        {
            _dictGame[EntityType.People][entity.level] = entity.count;
        }


        int maxAmountBaggage = _dictGame[EntityType.Baggage][roundCount];
        int maxAmountPeople = _dictGame[EntityType.People][roundCount];
        int numSpots = spotsList.Count;

        int baggagePerSpot = maxAmountBaggage / numSpots;
        int peoplePerSpot = maxAmountPeople / numSpots;

        int remainingBaggage = maxAmountBaggage % numSpots;
        int remainingPeople = maxAmountPeople % numSpots;

        foreach (Spot spot in spotsList)
        {
            spot.amountBaggage = baggagePerSpot;
            spot.amountPeople = peoplePerSpot;

            if (remainingBaggage > 0)
            {
                spot.amountBaggage++;
                remainingBaggage--;
            }

            if (remainingPeople > 0)
            {
                spot.amountPeople++;
                remainingPeople--;
            }
        }

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
    public int GetCountPerRoundByEntityType(EntityType entityType)
    {
        return _dictGame[entityType][roundCount];
    }
}
