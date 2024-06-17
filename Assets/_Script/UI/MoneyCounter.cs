using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    int currentMoney;
    public int CurrentMoney { get { return currentMoney; } set { currentMoney = value; } }
    [SerializeField] int startingMoney = 1000;

    private TextMeshProUGUI TextComponent;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform coinSapwn;

    private void OnEnable()
    {
        Spot.OnScorePoint += IncreaseMoney;
        GameManager.OnLoseRound += () =>
        {
            currentMoney = startingMoney;
            TextComponent.text = currentMoney.ToString() + " $";
        };
    }

    private void OnDisable()
    {
        Spot.OnScorePoint -= IncreaseMoney;
    }

    void Start()
    {
        TextComponent = GetComponent<TextMeshProUGUI>();
        currentMoney = startingMoney;
        TextComponent.text = currentMoney + " $";
    }

    public void IncreaseMoney(int MoneyFromQuest)
    {
        currentMoney += MoneyFromQuest;
        TextComponent.text = currentMoney.ToString() + " $";
        float rnaodm = Random.Range(50, 80);
        if (coinPrefab && coinSapwn)
        {
            Vector3 spean = new Vector3(coinSapwn.position.x + rnaodm, coinSapwn.position.y + rnaodm, coinSapwn.position.z + rnaodm);

            GameObject onk = Instantiate(coinPrefab, coinSapwn);
            onk.transform.position = spean;
        }
    }

    public void DecreaseMoney(int MoneySpent)
    {
        if (currentMoney >= MoneySpent)
        {
            currentMoney -= MoneySpent;
            TextComponent.text = currentMoney.ToString() + " $";
        }
    }

}
