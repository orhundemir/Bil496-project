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
    // TODO: Send the floor and ceiling to both the server and the next scene
    public void SendRoomTemplate(GameObject[] walls, GameObject ceiling, GameObject floor) {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.roomTemplate);
        message.AddInt(walls.Length);

        Player playerCopy = Player.list[NetworkManager.Singleton.Client.Id];
        playerCopy.Walls = walls;

        foreach (GameObject wall in playerCopy.Walls)
        {
            message.AddVector3(wall.transform.position);
            message.AddQuaternion(wall.transform.rotation);
            message.AddVector3(wall.transform.localScale);
        }

        NetworkManager.Singleton.Client.Send(message);
        Player.MovePlayerToDestinationScene(NetworkManager.Singleton.Client.Id, "VReal");        
    }

    // This is called from the on-click action of the Next button from the RoomDrawingScene
    // TODO: Scale and position the doors and windows to fit the new wall heights
    public void ClickedNext() {
        GameObject[] walls = GetFinalWallList();

        float wallHeight = walls[0].transform.parent.GetComponent<WallObject>().GetFinalHeight();
        float ceilingAndFloorHeight = 0.3f;

        GameObject roomCeiling = CreateRoomCeiling(ceilingAndFloorHeight);
        GameObject roomFloor = CreateRoomFloor(roomCeiling.transform.localScale, roomCeiling.transform.position);

        // Place the ceiling above the walls
        roomCeiling.transform.position += new Vector3(0f, wallHeight / 2f + ceilingAndFloorHeight / 2f, 0f);
        // Place the floor below the walls
        roomFloor.transform.position -= new Vector3(0f, ceilingAndFloorHeight / 2f, 0f);

        SendRoomTemplate(walls, roomCeiling, roomFloor);
    }

    // Scale the walls in the y direction to give them their final height and return them as a list
    private GameObject[] GetFinalWallList()
    {
        GameObject[] walls = new GameObject[rootWall.transform.childCount - 1];

        for (int i = 1; i < rootWall.transform.childCount; i++)
        {
            Transform wallPrefab = rootWall.transform.GetChild(i);
            GameObject wallScaler = wallPrefab.GetChild(0).gameObject;
            GameObject wall = wallScaler.transform.GetChild(0).gameObject;
            wall.name = "Wall Shape";
            wallScaler.name = "Wall";

            Vector3 scale = wallScaler.transform.localScale;
            float wallHeight = wallPrefab.GetComponent<WallObject>().GetFinalHeight();

            wallScaler.transform.localScale = new Vector3(scale.x, wallHeight / 2f, scale.z);
            wallScaler.transform.position = wallPrefab.position + new Vector3(0f, wallHeight / 4f, 0f);
            wallScaler.transform.rotation = wallPrefab.rotation;

            walls[i - 1] = wallScaler;
        }

        return walls;
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

        ceiling.name = "Ceiling";
        Player.list[NetworkManager.Singleton.Client.Id].RoomCenter = roomCenter;
        Player.list[NetworkManager.Singleton.Client.Id].Ceiling=ceiling;
        return ceiling;
    }

    private GameObject CreateRoomFloor(Vector3 floorScale, Vector3 floorPosition)
    {
        GameObject floor = Instantiate(floorPrefab, floorPosition, Quaternion.identity);
        floor.transform.localScale = floorScale;
        floor.GetComponent<Renderer>().material = drawingArea.GetComponent<Renderer>().material;

        floor.name = "Floor";
        Player.list[NetworkManager.Singleton.Client.Id].Floor = floor;
        return floor;
    }

}
