using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] private GameObject rootWall;
    [SerializeField] private GameObject drawingArea;
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private GameObject floorPrefab;

    private void Awake() {
        Singleton = this;
    }

    // Sends the walls to the VR scene, which comes after the Room Drawing Scene
    // Also sends the wall info to the server to be saved for later
    public void SendRoomTemplate(GameObject ceiling, GameObject floor) {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.roomTemplate);
        message.AddInt(rootWall.transform.childCount - 1);

        Player temp = Player.list[NetworkManager.Singleton.Client.Id];
        temp.Walls = new GameObject[rootWall.transform.childCount - 1];
        for (int i = 1; i < rootWall.transform.childCount; i++)
        {
            Transform wallPrefab = rootWall.transform.GetChild(i);
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

    // This is called from the on-click action of the Next button from the RoomDrawingScene
    public void ClickedNext() {
        // TODO: Scale the walls in the y direction to set their final heights
        // TODO: Place the floor below and ceiling above the walls
        // TODO: Send the ceiling and the floor to both the server and the next scene
        GameObject roomCeiling = CreateRoomCeiling(0.5f);
        GameObject roomFloor = CreateRoomFloor(roomCeiling.transform.localScale, roomCeiling.transform.position);

        SendRoomTemplate(roomCeiling, roomFloor);
    }

    private GameObject CreateRoomCeiling(float ceilingHeight)
    {
        List<Vector3> hingePositions = new List<Vector3>();
        for (int i = 1; i < rootWall.transform.childCount; i++)
        {
            Transform wall = rootWall.transform.GetChild(i);

            Transform hinge1 = wall.transform.Find("Hinge1");
            Transform hinge2 = wall.transform.Find("Hinge2");
            hingePositions.Add(hinge1.position);
            hingePositions.Add(hinge2.position);
        }

        // Find the minimum and maximum x and z coordinates
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minZ = float.MaxValue;
        float maxZ = float.MinValue;
        foreach (var pos in hingePositions)
        {
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.z < minZ) minZ = pos.z;
            if (pos.z > maxZ) maxZ = pos.z;
        }

        // Create a new object using the ceiling prefab which has the calculated boundary values
        Vector3 ceilingScale = new Vector3(maxX - minX, ceilingHeight, maxZ - minZ);
        Vector3 roomCenter = new Vector3((maxX + minX) / 2f, 0f, (maxZ + minZ) / 2f);

        GameObject ceiling = Instantiate(ceilingPrefab, roomCenter, Quaternion.identity);
        ceiling.transform.localScale = ceilingScale;

        return ceiling;
    }

    private GameObject CreateRoomFloor(Vector3 floorScale, Vector3 floorPosition)
    {
        GameObject floor = Instantiate(floorPrefab, floorPosition, Quaternion.identity);
        floor.transform.localScale = floorScale;
        floor.GetComponent<Renderer>().material = drawingArea.GetComponent<Renderer>().material;

        return floor;
    }


}
