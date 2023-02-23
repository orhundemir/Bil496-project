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
    [SerializeField] private GameObject DrawingArea;
    [SerializeField] private Player localPlayer;


    private void Awake() {
        Singleton = this;
    }

    public void SendRoomTemplate() {
        
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.roomTemplate);
        message.AddInt(RootWall.transform.childCount - 1);
        Player tmp = Player.list[NetworkManager.Singleton.Client.Id];
        tmp.Walls = new GameObject[RootWall.transform.childCount - 1];
        for (int i = 1; i < RootWall.transform.childCount; i++)
        {
            tmp.Walls[i - 1] = RootWall.transform.GetChild(i).gameObject;
            Vector3 position = RootWall.transform.GetChild(i).gameObject.transform.position;
            Quaternion quaternion = RootWall.transform.GetChild(i).gameObject.transform.rotation;
            message.AddVector3(position);
            message.AddQuaternion(quaternion);
        }
        NetworkManager.Singleton.Client.Send(message);
        Player.MovePlayerToDestinationScene(NetworkManager.Singleton.Client.Id, "VReal");        
    }

    public void ClickedNext() {
        
        SendRoomTemplate();

    }


}
