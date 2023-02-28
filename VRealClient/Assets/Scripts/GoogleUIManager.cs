using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private InputField usernameField; //FIX ME: Google Api entegrasyonuna bağlı olarak ihtiyac olmayabilir.
    [SerializeField] private InputField passwordField; //FIX ME: Google Api entegrasyonuna bağlı olarak ihtiyac olmayabilir.

    private void Awake()
    {
        Singleton = this;
    }

    public void GoogleConnectClicked()
    {
        ConnectGoogle();
    }


    public void ConnectGoogle()
    {
        //TO DO Google Sign in burada yapılmalıdır.

        //FIX ME authentication degiskene baglanmalidir ve parametreler Googledan gelen bilgiye göre düzenlenmelidir. 
        if (true) 
        {
            usernameField.interactable = false;
            passwordField.interactable = false;
            googleUI.SetActive(false);

            SendGoogleEmail("example@gmail.com");
            SendGoogleNameSurname("Ali Veli Selami");
            SendGoogleUID("asasd541654awdad_-97dwalsdj");

        }
        //ELSE
            // usernameField.interactable = false;
            // passwordField.interactable = false;
            // googleUI.SetActive(false);
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
