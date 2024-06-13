using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{

    private TextMeshProUGUI TextComponent;
    public int Money;
    
    // Start is called before the first frame update
    void Start()
    {
        TextComponent = GetComponent<TextMeshProUGUI>();
        Money = 1000;
        TextComponent.text = Money + " $";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call when the player earns Money, with the money amount
    public void IncreaseMoney(int MoneyFromQuest)
    {
        Money += MoneyFromQuest;
        TextComponent.text = Money.ToString() + " $";
    }

    // Call when the player Spends Money, with the money amount
    public void DecreaseMoney(int MoneySpent)
    {
        if(Money >= MoneySpent)
        {
            Money -= MoneySpent;
            TextComponent.text = Money.ToString() + " $";
        }
    }

}
