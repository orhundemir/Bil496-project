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


    private void Awake() {
        Singleton = this;
    }

    //Cizilen duvarları VR sahnesine gonderilmesini saglıyor.
    //Duvar bilgilerini servera yolluyor ve Vreal sahnesine geciyor.
    public void SendRoomTemplate() {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.roomTemplate);
        message.AddInt(RootWall.transform.childCount - 1);

        Player temp = Player.list[NetworkManager.Singleton.Client.Id];
        temp.Walls = new GameObject[RootWall.transform.childCount - 1];
        for (int i = 1; i < RootWall.transform.childCount; i++)
        {
            Transform wallPrefab = RootWall.transform.GetChild(i);
            GameObject actualWall = wallPrefab.GetChild(0).gameObject;
            actualWall.transform.position = wallPrefab.transform.position;
            actualWall.transform.rotation = wallPrefab.transform.rotation;
            temp.Walls[i - 1] = actualWall;
           
            Vector3 position = actualWall.transform.position;
            Quaternion quaternion = actualWall.transform.rotation;
            Vector3 scale = actualWall.transform.localScale;

            message.AddVector3(position);
            message.AddQuaternion(quaternion);
            message.AddVector3(scale);
        }
        NetworkManager.Singleton.Client.Send(message);
        Player.MovePlayerToDestinationScene(NetworkManager.Singleton.Client.Id, "VReal");        
    }

    // Cizim ekranında Next tusunun listenerıdır.
    public void ClickedNext() {
        SendRoomTemplate();
    }


}
