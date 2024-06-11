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
        SteamFriends.OnGameLobbyJoinRequested += GameLobbyJoinRequest;
    }
    private void OnDisable()
    {
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested -= GameLobbyJoinRequest;
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
        if (NetworkManager.Singleton.IsHost)
        {
            return;
        }
        NetworkManager.Singleton.StartClient();
    }
    private async void GameLobbyJoinRequest(Lobby lobby, SteamId steamId)
    {
        RoomEnter joinedLobby = await lobby.Join();
        if (joinedLobby != RoomEnter.Success)
        {
            Debug.LogError("Failed to Join Lobby");
        }
        else
        {
            if (currentLobby != null)
            {
                NetworkManager.Singleton.Shutdown();
            }
            currentLobby = lobby;
        }
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
