using Steamworks;
using Steamworks.Data;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class SteamUI : NetworkBehaviour
{
    public static SteamUI Instance;
    //InLobby Friend
    [SerializeField] GameObject PlayerInfoUI;
    [SerializeField] GameObject PlayersListContent;
    private List<PlayerInfo> playersInfos = new List<PlayerInfo>();


    //Steam friend display
    [SerializeField] GameObject FriendInfoUI;
    [SerializeField] GameObject FriendListContent;
    private List<FriendInfo> friends = new List<FriendInfo>();

    [SerializeField] Button readyBtn;
    [SerializeField] GameObject startGameBtn;
    [SerializeField] GameObject hostGameBtn;
    [SerializeField] GameObject leaveLobbyBtn;

    private int readyCounter;
    public int ReadyCounter
    {
        get => readyCounter;
        private set
        {
            readyCounter = value;
            startGameBtn.SetActive(readyCounter >= playersInfos.Count);
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        foreach (Friend friend in SteamFriends.GetFriends())
        {
            string name = friend.Name;
            GameObject friendInfo = Instantiate(FriendInfoUI, FriendListContent.transform);
            FriendInfo scritpFriendInfo = friendInfo.GetComponent<FriendInfo>();
            scritpFriendInfo.Init(friend);
            friends.Add(scritpFriendInfo);
        }
        InvokeRepeating("UpdateFriends", 0, 1f);
    }

    private void UpdateFriends()
    {
        foreach (FriendInfo script in friends)
        {
            script.IsOnline = script.friend.IsOnline;
        }
    }

    public void UpdatePlayersList()
    {
        playersInfos.Clear();
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
                PlayerInfo pl = playerInfoUI.GetComponent<PlayerInfo>();
                pl.Init(name, friend.Id);
                playersInfos.Add(pl);

            }
            ReadyCounter = 0;
        }
    }

    public void Ready()
    {
        OnReadyBTNServerRPC(Steamworks.SteamClient.SteamId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnReadyBTNServerRPC(ulong steamId)
    {
        OnReadyBTNClientRPC(steamId);
    }

    [ClientRpc]
    public void OnReadyBTNClientRPC(ulong steamId)
    {
        foreach (var item in playersInfos)
        {
            if (item.SteamId == steamId)
            {
                item.IsReady = !item.IsReady;
                if (IsHost)
                {
                    ReadyCounter += item.IsReady ? 1 : -1;
                }
                return;
            }
        }
    }
    public void ResetButtons()
    {
        readyBtn.gameObject.SetActive(false);
        startGameBtn.SetActive(false);
        hostGameBtn.SetActive(true);
        leaveLobbyBtn.SetActive(false);
        ReadyCounter = 0;
    }

    public void OnMemberEnterButtons()
    {
        readyBtn.gameObject.SetActive(true);
        startGameBtn.SetActive(false);
        hostGameBtn.SetActive(false);
        leaveLobbyBtn.SetActive(true);
    }
}
