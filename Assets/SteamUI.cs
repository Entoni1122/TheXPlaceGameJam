using Steamworks;
using Steamworks.Data;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SteamUI : NetworkBehaviour
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


    [ServerRpc]
    public void UpdatePlayersListServerRPC()
    {
        for (int i = PlayersListContent.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(PlayersListContent.transform.GetChild(i).gameObject);
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
