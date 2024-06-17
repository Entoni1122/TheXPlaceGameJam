using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class BuyEquipment : MonoBehaviour
{
    public int Cost;
    public GameObject ActorPrefab;
    public Transform SpawnPoint;

    private TextMeshProUGUI TextComponent;
    private TextMeshProUGUI MoneyTxt;
    private MoneyCounter MoneyCounterScript;

    public static event Action upgrade;

    void Start()
    {
        TextComponent = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        string CostString = TextComponent.text.TrimEnd('$');
        if (int.TryParse(CostString, out int parsedCost))
        {
            Cost = parsedCost;
        }

        GameObject MoneyCounterGameObject = GameObject.FindGameObjectWithTag("Money");
        MoneyTxt = MoneyCounterGameObject.GetComponent<TextMeshProUGUI>();
        MoneyCounterScript = MoneyTxt.GetComponent<MoneyCounter>();
    }
    public void SpawnActor()
    {
        if (ActorPrefab != null && SpawnPoint != null && MoneyCounterScript.CurrentMoney >= Cost)
        {
            MoneyCounterScript.DecreaseMoney(Cost);
            Instantiate(ActorPrefab, SpawnPoint.position, SpawnPoint.rotation);
        }
    }

    public void SpawnCarrelloUpgrade()
    {
        upgrade?.Invoke();
    }
}
