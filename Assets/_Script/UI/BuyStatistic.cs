using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BuyStatistic : MonoBehaviour
{
    
    public int Cost;

    //name of the stat to modify
    public string StatName;

    //playerStat
    private PlayerStats PlayerScript;

    //MoneyCounter for increase decrease money
    private TextMeshProUGUI TextComponent;
    private MoneyCounter MoneyCounterScript;
    private TextMeshProUGUI MoneyTxt;


    void Start()
    {
        //take the first child that is a Txt and we take the cost
        TextComponent = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        string CostString = TextComponent.text.TrimEnd('$');
        if (int.TryParse(CostString, out int parsedCost))
        {
            Cost = parsedCost;
        }

        //search of the moneyCounter by tag
        GameObject MoneyCounterGameObject = GameObject.FindGameObjectWithTag("Money");
        MoneyTxt = MoneyCounterGameObject.GetComponent<TextMeshProUGUI>();
        MoneyCounterScript = MoneyTxt.GetComponent<MoneyCounter>();

        //search of the player by tag
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent<PlayerStats>();
    }

    //impost on button click
    public void IncreseStat()
    {
        if (MoneyCounterScript.Money >= Cost)
        {
            MoneyCounterScript.DecreaseMoney(Cost);
            PlayerScript.IncreseStats(StatName);
        }
    }

}
