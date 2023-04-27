using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using SlimUI.ModernMenu;
using System.Threading.Tasks;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Buttons")]
    public GameObject googleSignIN;
    public GameObject connectToServer;
    public GameObject welcome;
    public GameObject settings;
    public GameObject exit;

    public GameObject newDesign;
    public TMP_Dropdown loadDesign;
    public TMP_InputField input;

    [Header("Server Disconnect Warning")]
    public GameObject warning;
    public GameObject failedAuthentication;


    private void Awake()
    {
        Singleton = this;
    }
    
    public void ConnectToServer()
    {
        NetworkManager.Singleton.Connect();
    }

    public void SendConnect()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.connect);
        message.AddBool(true);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendRoomName()
    {
        
            Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.roomName);
            message.AddString(input.text);
            NetworkManager.Singleton.Client.Send(message);
    }

    public void SendPrevRoomName(string item)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.prevRoomName);
        //message.AddString(input.text);
        //NetworkManager.Singleton.Client.Send(message);
    }

    public void SendGoogleEmail(string _email)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.googleEmail);
        message.AddString(_email);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendGoogleUID(string _uid)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.googleUID);
        message.AddString(_uid);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void ClickedLoadPrevRoom()
    {
        Debug.Log("room");
        //SendPrevRoomName(loadDesign.itemText.text);
    }
    public void LoadRoomClicked()
    {
        loadDesign.gameObject.SetActive(true);
        Player.roomsArr.Add("a");
        Player.roomsArr.Add("b");
        Player.roomsArr.Add("c");
        Player.roomsArr.Add("d");
        loadDesign.AddOptions(Player.roomsArr);
    }

    public void NewDesignClicked()
    {
        loadDesign.gameObject.SetActive(false);
        newDesign.SetActive(true);
    }

    public void NextClicked()
    {
        if (input.text != null)
        {
            SendRoomName();
            Player player = Player.list[NetworkManager.Singleton.Client.Id];
            player.RoomName = input.text;
            newDesign.SetActive(false);
            player.gameObject.SetActive(true);
            Player.MovePlayerToDestinationScene(player.Id, "RoomDrawing");
        }
        
    }
    public void CancelClicked()
    {
        input.text = "";
        newDesign.SetActive(false);
    }

    public void GoogleConnectClicked()
    {
        GoogleAuthenticator.GetAuthCode();
        _ = ConnectGoogleAsync();
    }

    public async Task ConnectGoogleAsync()
    {
        float timer = 0;
        while (!FirebaseAuthHandler.verified)
        {
            await Task.Delay(100); //100ms bekle
            timer += Time.timeScale;
            if (timer >= 120)
            {
                break;
            }
        }

        if (FirebaseAuthHandler.verified)
        {
            SendGoogleEmail(FirebaseAuthHandler.email);
            SendGoogleUID(FirebaseAuthHandler.userId);
            //Debug.Log(FirebaseAuthHandler.email);
            googleSignIN.SetActive(false);
            welcome.SetActive(true);
            settings.SetActive(true);
            failedAuthentication.SetActive(false);
            Debug.Log("Google Sign In Connection is succesfull.");

        }
        else
        {
            googleSignIN.SetActive(true);
            welcome.SetActive(false);
            settings.SetActive(false);
            failedAuthentication.SetActive(true);
            Debug.Log("Google Sign In Failed");
        }
    }

    
}