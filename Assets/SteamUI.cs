using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SteamUI : NetworkBehaviour
{
    public static SteamUI Instance;

    [SerializeField] private GameObject PlayerInfoUI;
    [SerializeField] private GameObject PlayersListContent;

    [SerializeField] private GameObject FriendInfoUI;
    [SerializeField] private GameObject FriendListContent;


    private List<FriendInfo> friends = new List<FriendInfo>();

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
