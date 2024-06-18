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
    public GameObject VFXPrefab;
    public Transform SpawnPoint;
    
    private TextMeshProUGUI TextComponent;
    private TextMeshProUGUI MoneyTxt;
    private MoneyCounter MoneyCounterScript;

    public static event Action upgrade;
    [SerializeField] GameObject lockImage;

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
            Instantiate(VFXPrefab, SpawnPoint.position, SpawnPoint.rotation);
            MoneyCounterScript.DecreaseMoney(Cost);
            Instantiate(ActorPrefab, SpawnPoint.position, SpawnPoint.rotation);
            GetComponent<Button>().enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            lockImage.SetActive(true);
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);

    }
    public void SpawnCarrelloUpgrade()
    {
        upgrade?.Invoke();
    }
}
