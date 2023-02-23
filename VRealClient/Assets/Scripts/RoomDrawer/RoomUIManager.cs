using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoomUIManager : MonoBehaviour {
    private static RoomUIManager _singleton;
    public static RoomUIManager Singleton {
        get => _singleton;
        private set {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value) {
                Debug.Log($"{nameof(RoomUIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("RoomTemplate")]
    [SerializeField] private GameObject RootWall;


    private void Awake() {
        Singleton = this;
    }

    public void SendRoomTemplate() {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.roomTemplate);
        for (int i = 0; i < RootWall.transform.childCount; i++) {

            Vector3 position = RootWall.transform.GetChild(i).gameObject.transform.position;
            Quaternion quaternion = RootWall.transform.GetChild(i).gameObject.transform.rotation;
            message.AddVector3(position);
            message.AddQuaternion(quaternion);


        }
        NetworkManager.Singleton.Client.Send(message);
    }

    public void ClickedNext() {
        Debug.Log("a");
        SendRoomTemplate();

    }
}
