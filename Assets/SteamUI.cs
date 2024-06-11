using Steamworks;
using Steamworks.Data;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SteamUI : MonoBehaviour
{
    public static SteamUI Instance;

    [SerializeField] private GameObject PlayerInfoUI;
    [SerializeField] private GameObject PlayersListContent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else { Destroy(gameObject); }
    }

    public void UpdatePlayersList()
    {
        for (int i = PlayersListContent.transform.childCount; i > 0; i--)
        {
            DestroyImmediate(PlayersListContent.transform.GetChild(i));
        }

        Lobby? lobby = SteamManager.Instance.currentLobby;
        if (lobby != null)
        {
            foreach (Friend friend in lobby.Value.Members)
            {
                string name = friend.Name;
                GameObject playerInfoUI = Instantiate(PlayerInfoUI, PlayersListContent.transform);

                playerInfoUI.GetComponent<PlayerInfo>().Init(name, friend.Id);
            }
        }
    }
}
