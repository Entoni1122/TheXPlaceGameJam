using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Action OnRoundStart;
    public static Action OnLoseRound;
    public static Action<bool> OnIntermissionCall;

    [Header("Gameplay")]
    [SerializeField] int roundCount;

    [SerializeField] int startTimerPerRound;
    [SerializeField] int intermissionTimer = 5;
    [SerializeField] TextMeshProUGUI timerTxt;
    private float currentTimer;
    public float GetTimerRound =>currentTimer;


    [SerializeField] int startBaggageToSpawn;
    [SerializeField] int startPeopleToSpawn;

    [SerializeField] List<Spot> spotsList;

    private List<EntityInfo> baggagePerSpot = new List<EntityInfo>();
    private List<EntityInfo> peoplePerSpot = new List<EntityInfo>();

    private int currentBaggageOnSpot;
    private int currentPeopleOnSpot;

    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        instance = this;

        OnLoseRound += () =>
        {
            roundCount = 0;
            StartGame();
        };
    }

    private void Start()
    {
        StartGame();

    }

    private void Update()
    {
        currentTimer -= Time.deltaTime;
        timerTxt.text = $"Timer:{currentTimer.ToString("0")}";
        if (currentTimer <= 0)
        {
            if (intermissionON) { return; }
            OnLoseRound?.Invoke();
        }
    }

    private void StartGame()
    {
        audioSource.Play();
        roundCount++;

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
                    int baggageInSpot = UnityEngine.Random.Range(0, maxBaggage);
                    spotsList[i].amountBaggage = baggageInSpot;
                    maxBaggage -= baggageInSpot;
                    infoBaggage.count = baggageInSpot;
                    infoBaggage.color = spotsList[i].GetColor;
                }
                if (maxPeople > 0)
                {
                    int peopleInSpot = UnityEngine.Random.Range(0, maxPeople);
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

        if (currentBaggageOnSpot >= startBaggageToSpawn * roundCount &&
            currentPeopleOnSpot >= startPeopleToSpawn * roundCount)
        {
            IntermissionTime();
        }
    }
    bool intermissionON;
    private async void IntermissionTime()
    {
        currentTimer = intermissionTimer;
        intermissionON = true;
        OnIntermissionCall?.Invoke(true);
        await Task.Delay(intermissionTimer * 1000);
        StartGame();
        intermissionON =false;
        OnIntermissionCall?.Invoke(false);
    }
}

public struct EntityInfo
{
    public int count;
    public ColorType color;
}
