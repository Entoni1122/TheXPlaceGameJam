using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMuletto : MonoBehaviour
{
    public static event Action upgrade;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SpawnCarrello);
    }


    void SpawnCarrello()
    {
        upgrade?.Invoke();
    }
}
