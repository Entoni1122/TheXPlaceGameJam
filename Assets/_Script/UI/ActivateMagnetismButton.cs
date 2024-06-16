using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateMagnetismButton : MonoBehaviour
{
    [SerializeField] private Button magnetismButton;
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        if (magnetismButton != null && playerStats != null)
        {
            magnetismButton.onClick.AddListener(playerStats.ToggleMagnetism);
        }
    }
}