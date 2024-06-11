using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using Netcode.Transports.Facepunch;

public class SteamManager : NetworkBehaviour
{
    public static SteamManager Instance;
    public Lobby? currentLobby;
    private FacepunchTransport steamTransport;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        steamTransport = NetworkManager.Singleton.transform.GetComponent<FacepunchTransport>();
    }

    private void OnEnable()
    {
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += OnMemberLobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += GameLobbyJoinRequest;
        SteamMatchmaking.OnLobbyMemberLeave += OnMemberLobbyLeave;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    private void OnDisable()
    {
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= OnMemberLobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested -= GameLobbyJoinRequest;
        SteamMatchmaking.OnLobbyMemberLeave -= OnMemberLobbyLeave;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }

    #region SteamCallbacks
    private void OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            lobby.SetPublic();
            lobby.SetJoinable(true);
            lobby.SetGameServer(lobby.Owner.Id);
        }
    }

    private void OnLobbyEntered(Lobby lobby)
    {
        currentLobby = lobby;
        SteamUI.Instance.UpdatePlayersList();
        if (NetworkManager.Singleton.IsHost)
        {
            return;
        }
        steamTransport.targetSteamId = lobby.Owner.Id;
        NetworkManager.Singleton.StartClient();
    }

    private void OnMemberLobbyEntered(Lobby lobby, Friend friend)
    {
        SteamUI.Instance.UpdatePlayersList();
    }

    private void OnMemberLobbyLeave(Lobby lobby, Friend friend)
    {
        print(friend.Name);
        SteamUI.Instance.UpdatePlayersList();
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

    #region ButtonFunctions
    public async void HostLobby()
    {
        NetworkManager.Singleton.StartHost();
        currentLobby = await SteamMatchmaking.CreateLobbyAsync(4);
    }

    public void LeaveLobby()
    {
        if (currentLobby != null)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                DisconnectAllClients();
            }
            else
            {
                NetworkManager.Singleton.Shutdown();
                currentLobby?.Leave();
                currentLobby = null;
            }
        }
        SteamUI.Instance.UpdatePlayersList();
    }
    #endregion

    public void DisconnectAllClients()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (clientId != NetworkManager.Singleton.LocalClientId)
                {
                    NetworkManager.Singleton.DisconnectClient(clientId);
                }
            }
        }
        NetworkManager.Singleton.Shutdown();
        currentLobby?.Leave();
        currentLobby = null;
        SteamUI.Instance.UpdatePlayersList();
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        SteamUI.Instance.UpdatePlayersList();
    }
}
