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

    // Sends the walls, windows and doors to the VR scene, which comes after the Room Drawing Scene
    // Also sends the wall info to the server to be saved for later
    // TODO: Send the floor and ceiling to the server
    public void SendRoomTemplate(List<GameObject> walls, GameObject ceiling, GameObject floor) {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.roomTemplate);
        message.AddInt(walls.Count);

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
    public void ClickedNext() {
        List<GameObject> walls = GetFinalWallList();
        if (walls.Count == 0)
            return;

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
    private List<GameObject> GetFinalWallList()
    {
        List<GameObject> walls = new();
        for (int i = 1; i < rootWall.transform.childCount; i++)
        {
            Transform wallPrefab = rootWall.transform.GetChild(i);
            if (!wallPrefab.CompareTag("WallObject"))
                continue;

            GameObject wallScaler = wallPrefab.GetChild(0).gameObject;
            GameObject wall = wallScaler.transform.GetChild(0).gameObject;
            wall.name = "Wall Shape";
            wallScaler.name = "Wall";

            Vector3 scale = wallScaler.transform.localScale;
            float wallHeight = wallPrefab.GetComponent<WallObject>().GetFinalHeight();
            wallScaler.transform.localScale = new Vector3(scale.x, wallHeight / 2f, scale.z);
            wallScaler.transform.position = wallPrefab.position + new Vector3(0f, wallHeight / 4f, 0f);
            wallScaler.transform.rotation = wallPrefab.rotation;

            List<GameObject> addOns = GetAdjustedAddOns(wallPrefab.gameObject, wallHeight / 2f, wallScaler.transform.localScale.x);
            walls.AddRange(addOns);
            walls.Add(wallScaler);
        }

        return walls;
    }

    // Rescales the doors and windows to fit the adjusted walls and returns them as a list
    private List<GameObject> GetAdjustedAddOns(GameObject wall, float wallHeight, float wallWidth)
    {
        List<GameObject> addOns = new List<GameObject>();
        float offsetWidth = wallWidth * 1.1f;

        for (int i = 0; i < wall.transform.childCount; i++)
        {
            GameObject child = wall.transform.GetChild(i).gameObject;
            if (child.transform.childCount == 0)
                continue;

            string childTag = child.transform.GetChild(0).tag;
            if (childTag != "Window" && childTag != "Door")
                continue;

            AddOnObject addOnObject = child.GetComponent<AddOnObject>();
            addOnObject.ChangeMaterialToRealistic();
            float addOnSize = addOnObject.GetLength();
            float doorHeight = wallHeight * 0.75f;
            Vector3 addOnScale = new Vector3(offsetWidth, childTag == "Door" ? doorHeight : addOnSize, addOnSize);

            Vector3 wallPos = wall.transform.GetChild(0).transform.position;
            Vector3 addOnPos = child.transform.position;
            float yOffset = childTag == "Door" ? (wallHeight - doorHeight) / 2 : 0f;

            child.transform.localScale = addOnScale;
            child.transform.position = new Vector3(addOnPos.x, wallPos.y - yOffset, addOnPos.z);

            child.name = childTag;
            child.transform.GetChild(0).name = $"{childTag} Shape";
            addOns.Add(child);
        }

        return addOns;
    }

    public GameObject CreateRoomCeiling(float ceilingHeight)
    {
        // Find the minimum and maximum x and z coordinates
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minZ = float.MaxValue;
        float maxZ = float.MinValue;
        for (int i = 0; i < rootWall.transform.childCount; i++)
        {
            if (!rootWall.transform.GetChild(i).CompareTag("WallObject"))
                continue;

            Transform hinge1 = rootWall.transform.GetChild(i).Find("Hinge1");
            Transform hinge2 = rootWall.transform.GetChild(i).Find("Hinge2");

            Vector3 pos1 = hinge1.position;
            Vector3 pos2 = hinge2.position;
            minX = Mathf.Min(minX, pos1.x, pos2.x);
            maxX = Mathf.Max(maxX, pos1.x, pos2.x);
            minZ = Mathf.Min(minZ, pos1.z, pos2.z);
            maxZ = Mathf.Max(maxZ, pos1.z, pos2.z);
        }

        // Create a new object using the ceiling prefab which has the calculated boundary values
        Vector3 ceilingScale = new Vector3(maxX - minX, ceilingHeight, maxZ - minZ);
        Vector3 roomCenter = new Vector3((maxX + minX) / 2f, 0f, (maxZ + minZ) / 2f);

        GameObject ceiling = Instantiate(ceilingPrefab, roomCenter, Quaternion.identity);
        ceiling.transform.localScale = ceilingScale;

        ceiling.name = "Ceiling";
        Player.list[NetworkManager.Singleton.Client.Id].RoomCenter = roomCenter;
        Player.list[NetworkManager.Singleton.Client.Id].Ceiling = ceiling;
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
