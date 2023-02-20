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
    [SerializeField] private GameObject googleUI;

    private void Awake()
    {
        Singleton = this;
        googleUI.SetActive(false);
    }

    public void ConnectClicked()
    {
        connectUI.SetActive(false);
        googleUI.SetActive(true);
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
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name=="Main")
        {
            googleUI.SetActive(false);
            connectUI.SetActive(true);
        }
        else
        {
            StartCoroutine(LoadYourAsyncScene());
            googleUI.SetActive(false);
            connectUI.SetActive(true);
        }
    }


    IEnumerator LoadYourAsyncScene()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}