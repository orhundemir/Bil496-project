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
            if(timer >= 120)
            {
                break;
            }
        }

        if (FirebaseAuthHandler.verified)
        {
            SendGoogleEmail(FirebaseAuthHandler.email);
            SendGoogleUID(FirebaseAuthHandler.userId);
            //Debug.Log(FirebaseAuthHandler.email);
            googleUI.SetActive(false);
            Player.MovePlayerToDestinationScene(NetworkManager.Singleton.Client.Id,"RoomDrawing");
        }
        else
        {
            ConnectionTimeOutText.SetActive(true);
            googleUI.SetActive(true);
            Debug.Log("Google Sign In Failed");
        }
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
}
