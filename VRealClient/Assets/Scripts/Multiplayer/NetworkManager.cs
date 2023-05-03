using Riptide;
using Riptide.Utils;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum ServerToClientId : ushort
{
    playerSpawned = 1,
    roomNames,
    roomTemplate,
}

public enum ClientToServerId : ushort
{
    connected=1,
    googleEmail,
    googleUID,
    roomName,
    prevRoomName,
    roomTemplate,
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client Client { get; private set; }

    [SerializeField] private string ip;
    [SerializeField] private ushort port;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Message.MaxPayloadSize = 64000;
        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate()
    {
        Client.Update();
        
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();

        Client.Connected -= DidConnect;
        Client.ConnectionFailed -= FailedToConnect;
        Client.ClientDisconnected -= PlayerLeft;
        Client.Disconnected -= DidDisconnect;
    }

    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
        Client.Connection.CanTimeout = false;
    }

    private void DidConnect(object sender, EventArgs e)
    {
        Player.SendConnectedACK();
        UIManager.Singleton.warning.SetActive(false);
        UIManager.Singleton.connectToServer.SetActive(false);
        UIManager.Singleton.googleSignIN.SetActive(true);
        Debug.Log(sender.ToString() + "is connected to server but it must sign in via Google");
    }

    private void FailedToConnect(object sender, ConnectionFailedEventArgs e)
    {
        Debug.LogWarning(sender.ToString() + "Failed to connect server.");
        UIManager.Singleton.connectToServer.SetActive(true);
        UIManager.Singleton.googleSignIN.SetActive(false);
        UIManager.Singleton.warning.SetActive(true);
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        Destroy(Player.list[e.Id].gameObject);
    }

    private void DidDisconnect(object sender, DisconnectedEventArgs e)
    {
        Debug.LogWarning(sender.ToString() + "Server connection down.");
        UIManager.Singleton.settings.SetActive(false);
        UIManager.Singleton.welcome.SetActive(false);
        UIManager.Singleton.connectToServer.SetActive(true);
        UIManager.Singleton.googleSignIN.SetActive(false);
        UIManager.Singleton.warning.SetActive(true);
    }

}
