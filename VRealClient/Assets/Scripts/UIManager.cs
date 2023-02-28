using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] public GameObject googleUI;

    private void Awake()
    {
        Singleton = this;
        googleUI.SetActive(false);
    }

    public void ConnectClicked()
    {
        connectUI.SetActive(false);
        NetworkManager.Singleton.Connect();
    }
    public void SendConnect()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.connect);
        message.AddBool(true);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void BackToMain()
    {
        googleUI.SetActive(false);
        connectUI.SetActive(true);
    }
}