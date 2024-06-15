using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Action OnRoundStart;
    public static Action OnLoseRound;

    [Header("Gameplay")]
    [SerializeField] int roundCount;

    [SerializeField] int startTimerPerRound;
    private float currentTimer;

    [SerializeField] int startBaggageToSpawn;
    [SerializeField] int startPeopleToSpawn;

    [SerializeField] List<Spot> spotsList;

    private List<EntityInfo> baggagePerSpot = new List<EntityInfo>();
    private List<EntityInfo> peoplePerSpot = new List<EntityInfo>();

    private int currentBaggageOnSpot;
    private int currentPeopleOnSpot;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetupGame();

        OnRoundStart += () =>
        {
            roundCount += 1;
            SetupGame();
        };

        OnLoseRound += () =>
        {
            roundCount = 0;
            OnRoundStart?.Invoke();
        };
    }
    private void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer <= 0)
        {
            OnLoseRound?.Invoke();
        }
    }


    private void SetupGame()
    {
        currentTimer = startTimerPerRound * roundCount;

        baggagePerSpot.Clear();
        peoplePerSpot.Clear();

        currentBaggageOnSpot = 0;
        currentPeopleOnSpot = 0;

        int maxBaggage = startBaggageToSpawn * roundCount;
        int maxPeople = startPeopleToSpawn * roundCount;

        for (int i = 0; i < spotsList.Count; i++)
        {
            EntityInfo infoBaggage = new EntityInfo();
            EntityInfo infoPeople = new EntityInfo();

            if (i < spotsList.Count - 1)
            {
                if (maxBaggage > 0)
                {
                    int baggageInSpot = UnityEngine.Random.Range(1, maxBaggage);
                    spotsList[i].amountBaggage = baggageInSpot;
                    maxBaggage -= baggageInSpot;
                    infoBaggage.count = baggageInSpot;
                    infoBaggage.color = spotsList[i].GetColor;
                }
                if (maxPeople > 0)
                {
                    int peopleInSpot = UnityEngine.Random.Range(1, maxPeople);
                    spotsList[i].amountPeople = peopleInSpot;
                    maxPeople -= peopleInSpot;
                    infoPeople.count = peopleInSpot;
                    infoPeople.color = spotsList[i].GetColor;
                }
            }
            else
            {
                spotsList[i].amountBaggage = maxBaggage;
                spotsList[i].amountPeople = maxPeople;
                infoBaggage.count = maxBaggage;
                infoBaggage.color = spotsList[i].GetColor;
                infoPeople.count = maxPeople;
                infoPeople.color = spotsList[i].GetColor;
            }
            baggagePerSpot.Add(infoBaggage);
            peoplePerSpot.Add(infoPeople);
        }
        OnRoundStart?.Invoke();
    }
    public List<EntityInfo> GetEnityInfoToSpawn(EntityType entityType)
    {
        return entityType == EntityType.Baggage ? baggagePerSpot : peoplePerSpot;
    }
    public void OnFullSpot(int amountBaggage, int amountPeople)
    {
        currentBaggageOnSpot += amountBaggage;
        currentPeopleOnSpot += amountPeople;

        if (currentBaggageOnSpot == startBaggageToSpawn * roundCount &&
            currentPeopleOnSpot == startPeopleToSpawn * roundCount)
        {
            OnRoundStart?.Invoke();
        }
    }
}

public struct EntityInfo
{
    public int count;
    public ColorType color;
}
