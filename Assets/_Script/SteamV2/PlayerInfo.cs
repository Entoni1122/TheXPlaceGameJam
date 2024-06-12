using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image readyImage;
    private ulong steamId;
    public ulong SteamId
    {
        get => steamId; 
    }

    private bool isReady;
    public bool IsReady
    {
        get => isReady; set 
        {
            isReady = value;
            readyImage.color = isReady ? Color.green : Color.red;
        }
    }

    public void Init(string name, ulong id)
    {
        steamId = id;
        playerName.text = name;
        IsReady = false;
    }
}

