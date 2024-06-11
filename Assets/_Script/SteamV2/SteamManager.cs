using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;

public class SteamManager : MonoBehaviour
{
    public static SteamManager Instance;
    public Lobby? currentLobby;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else { Destroy(gameObject); }
    }
    private void OnEnable()
    {
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
    }
    private void OnDisable()
    {
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
    }
    #region SteamCallbacks
    private void OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            print("Lobby Created");
            lobby.SetPublic();
            lobby.SetJoinable(true);
            lobby.SetGameServer(lobby.Owner.Id);
        }
    }
    private void OnLobbyEntered(Lobby lobby)
    {
        currentLobby = lobby;
        print("Joined Lobby");
        SteamUI.Instance.UpdatePlayersList();
    }
    #endregion
    #region ButtonFuncionts
    public async void HostLobby()
    {
        NetworkManager.Singleton.StartHost();
        currentLobby = await SteamMatchmaking.CreateLobbyAsync(4);
    }
    public void LeaveLobby()
    {
        if (currentLobby != null)
        {
            NetworkManager.Singleton.Shutdown();
        }
        print("LeaveLobby");
        SteamUI.Instance.UpdatePlayersList();
    }
    #endregion

}
