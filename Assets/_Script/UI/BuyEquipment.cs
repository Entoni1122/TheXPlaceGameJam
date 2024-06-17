using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BuyEquipment : MonoBehaviour
{

    public int Cost;
    public GameObject ActorPrefab;
    public Transform SpawnPoint;

    //MoneyCounter for increase decrease money
    private TextMeshProUGUI TextComponent;
    private TextMeshProUGUI MoneyTxt;
    private MoneyCounter MoneyCounterScript;

    // Start is called before the first frame update
    void Start()
    {
        //take the first child that is a Txt and we take the cost
        TextComponent = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        string CostString = TextComponent.text.TrimEnd('$');
        if (int.TryParse(CostString, out int parsedCost))
        {
            Cost = parsedCost;
        }

        //search of the moneyCounter by Tag
        GameObject MoneyCounterGameObject = GameObject.FindGameObjectWithTag("Money");
        MoneyTxt = MoneyCounterGameObject.GetComponent<TextMeshProUGUI>();
        MoneyCounterScript = MoneyTxt.GetComponent<MoneyCounter>();
    }

    //impost on button click
    public void SpawnActor()
    {
        if (ActorPrefab != null && SpawnPoint != null && MoneyCounterScript.CurrentMoney >= Cost)
        {
            MoneyCounterScript.DecreaseMoney(Cost);
            Instantiate(ActorPrefab, SpawnPoint.position, SpawnPoint.rotation);
        }
    }
}
