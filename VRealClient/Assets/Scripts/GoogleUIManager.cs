using RiptideNetworking;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoogleUIManager : MonoBehaviour
{ 
    private static GoogleUIManager _singleton;
    public static GoogleUIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GoogleUIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("ConnectGoogle")]
      [SerializeField] private GameObject googleUI;
      [SerializeField] private GameObject ConnectionTimeOutText;

    private void Awake()
    {
        Singleton = this;
    }

    public void GoogleConnectClicked()
    {
        GoogleAuthenticator.GetAuthCode();
        ConnectGoogleAsync();
    }

    public async Task ConnectGoogleAsync()
    {
        float timer = 0;
        while (!FirebaseAuthHandler.verified)
        {
            await Task.Delay(100); //100ms bekle
            timer += Time.timeScale;
            if(timer >= 300)
            {
                break;
            }
        }

        if (FirebaseAuthHandler.verified)
        {
            SendGoogleUID(FirebaseAuthHandler.userId);
            SendGoogleEmail(FirebaseAuthHandler.email);
            //SendGoogleNameSurname(FirebaseAuthHandler.name);
            //Debug.Log(FirebaseAuthHandler.email);
            googleUI.SetActive(false);
            SceneManager.LoadScene("RoomDrawing");
        }
        else
        {
            ConnectionTimeOutText.SetActive(true);
            googleUI.SetActive(true);
            Debug.Log("Failed");
        }
    }

    public void SendGoogleEmail(string _email)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.googleEmail);
        message.AddString(_email);
        NetworkManager.Singleton.Client.Send(message);
    }
    public void SendGoogleNameSurname(string _nameSurname)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.googleNameSurname);
        message.AddString(_nameSurname);
        NetworkManager.Singleton.Client.Send(message);
    }
    public void SendGoogleUID(string _uid)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.googleUID);
        message.AddString(_uid);
        NetworkManager.Singleton.Client.Send(message);
    }
}
