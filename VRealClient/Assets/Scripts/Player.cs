using HTC.UnityPlugin.Vive;
using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Email { get;  set; }
    public string Uid { get;  set; }
    public string RoomName { get; set; }
    public List<GameObject> Walls { get; set; }
    public List<GameObject> Products { get; set; }
    public GameObject Floor { get; set; }  
    public GameObject Ceiling { get; set; }
    public Vector3 RoomCenter { get; set; }

    public static List<string> roomsArr = new List<string>();



    private void OnDestroy()
    {
        list.Remove(Id);
    }

    // Vr sahnesinde duvar objelerinin yaratýlmasýný saðlýyor.
    public void SpawnWalls(GameObject wall, Transform parentObject)
    {
        GameObject wallObject = Instantiate(wall, wall.transform.position, wall.transform.rotation);
        wallObject.transform.parent = parentObject;
    }

    // Client basarili þekilde Sign in oldu. Player spawn oluyor.
    // Þimdilik basit bir capsule oluþuyor ihtiyaca göre deðiþebilir.
    public static void Spawn(ushort id, string _email, string _uid, Vector3 position)
    {
        position.x = -5.63f;
        position.y = 0.8f;
        Player player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
        player.Id = id;
        player.name = _email;
        player.Email = _email;
        player.Uid = _uid;
        list[id] = player;
        player.gameObject.SetActive(false);
    }


    public static void AssingRoomObjectsToPlayer(Message message, List<GameObject> Walls, List<GameObject> Products, GameObject Floor, GameObject Ceiling, Vector3 RoomCenter)
    {
        // string wallString = message.GetString();
        // string productString = message.GetString();
        string wallString = "0-_-1,547622-_-1,124998-_-2,189365-_-0-_-111,1896-_-0-_-0,6600001-_-2,25-_-1,5-_-Door Material 1 (Instance)***0-_--1,377494-_-1,499998-_-3,323329-_-0-_-111,1896-_-0-_-0,6600001-_-1,8-_-1,8-_-WindowMaterial (Instance)***0-_--12,132-_-1,499998-_-7,492476-_-0-_-111,1896-_-0-_-0,6000001-_-3-_-16,85881-_-Opaque Wall (Instance)***1-_--4,272511-_-3,15-_-4,445627-_-0-_-0-_-0-_-15,71897-_-0,3-_-6,093699-_-Ceiling Material (Instance)***1-_--4,272511-_--0,15-_-4,445627-_-0-_-0-_-0-_-15,71897-_-0,3-_-6,093699-_-Drawing Area (Instance) (Instance)***";
        string productString = "-4,114692-_--0,08685063-_-5,339195-_--3,235102E-05-_-10,49509-_--0,0009581303-_-0,002-_-0,002-_-0,002-_-alex-drawer-unit-black-brown-60473548(Clone)***-4,542412-_--0,09788189-_-4,457348-_-291,4928-_-311,3912-_-96,55825-_-0,02-_-0,02-_-0,02-_-alex-drawer-unit-on-casters-white-80485423(Clone)***-4,557808-_--0,09804774-_-4,446943-_-336,5148-_-0,07229744-_-14,11501-_-0,02-_-0,02-_-0,02-_-hemnes-3-drawer-chest-white-stain-70360414(Clone)***";

        string[] walls = wallString.Split("***");
        string[] products = productString.Split("***");

        foreach (string wall in walls)
        {
            string[] wallData = wall.Split("-_-");

            // Parse the received data
            int type = int.Parse(wallData[0]);
            Vector3 position = new Vector3(float.Parse(wallData[1]), float.Parse(wallData[2]), float.Parse(wallData[3]));
            Vector3 rotation = new Vector3(float.Parse(wallData[4]), float.Parse(wallData[5]), float.Parse(wallData[6]));
            Vector3 scale = new Vector3(float.Parse(wallData[7]), float.Parse(wallData[8]), float.Parse(wallData[9]));
            Material material = Resources.Load<Material>("Materials/RoomDrawer/" + wallData[10].Replace("(Instance)", "").Trim());

            // Create the cube object that contains all the necessary components
            GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MeshRenderer meshRenderer = cubeObject.GetComponent<MeshRenderer>();
            meshRenderer.material = material;
            cubeObject.AddComponent<BoxCollider>();
            Rigidbody rigidBody = cubeObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;

            // Assign the object's tag
            if (type == 0)
            {
                cubeObject.tag = "Wall";
                cubeObject.transform.position = new Vector3(0f, 0f, 0.5f);
            }
            else if (type == 1) cubeObject.tag = "Window";
            else if (type == 2) cubeObject.tag = "Door";
            else if (type == 3) cubeObject.tag = "Ceiling";
            else if (type == 4) cubeObject.tag = "Floor";

            // Create the wall/window/door objects
            if (type == 0 || type == 1 || type == 2)
            {
                // Create the parent object with the parsed data
                GameObject parentObject = new GameObject();
                parentObject.transform.position = position;
                parentObject.transform.eulerAngles = rotation;
                parentObject.transform.localScale = scale;
                parentObject.tag = cubeObject.tag;
                cubeObject.transform.parent = parentObject.transform;

                // Add the object to the current Player's Walls list
                Walls.Add(parentObject);
            }
            // Create the ceiling/floor objects
            else if (type == 3 || type == 4)
            {
                cubeObject.transform.position = position;
                cubeObject.transform.eulerAngles = rotation;
                cubeObject.transform.localScale = scale;

                // Assign the object to the current Player's corresponding field
                if (type == 3)
                {
                    Ceiling = cubeObject;
                    RoomCenter = Ceiling.transform.position;
                }
                else if (type == 4)
                {
                    cubeObject.AddComponent<Teleportable>();
                    Floor = cubeObject;
                }
            }
        }
        
        MovePlayerToDestinationScene(list[NetworkManager.Singleton.Client.Id].Id, "VReal");
    }


    #region SendMessages
    public static void SendConnectedACK()
    {
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.connected);
        message.AddBool(true);
        NetworkManager.Singleton.Client.Send(message);
    }
    #endregion


    #region Messages
    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetString(), message.GetVector3());
        Debug.Log("Client connected to server successfully");
    }

    [MessageHandler((ushort)ServerToClientId.roomNames)]
    private static void RoomNames(Message message)
    {

        // Oda isimleri ana menüde listelenecek
        
        roomsArr.Add("a");
        roomsArr.Add("a");
        roomsArr.Add("a");
        roomsArr.Add("a");

        
        Debug.Log("Server oda isimlerini gönderdi.");   
    }

    [MessageHandler((ushort)ServerToClientId.roomTemplate)]
    private static void RoomTemplate(Message message)
    {
        Debug.Log("Server oda objelerini gönderdi.");
        AssingRoomObjectsToPlayer(message);

    }
    #endregion




    #region ScenePassingProcess
    public static void MovePlayerToDestinationScene(ushort id, string destinationScene)
    {
        list[id].StartCoroutine(LoadYourAsyncScene(id, destinationScene));
    }

    // Oyuncu objesini VR gozluge uyumlu objeye ceviriyor
    // Ya da vr sahnesinden cizim ekranina gecmek icin kullanilabilir
    public void CopyPlayer(Player player)
    {
        player.name = this.Email;
        player.Id = NetworkManager.Singleton.Client.Id;
        player.Email = this.Email;
        player.Uid = this.Uid;
        player.RoomName = this.RoomName;

        int size = this.Walls.Count;
        player.Walls = new List<GameObject>(size);
        for (int i = 0; i < size; i++)
        {
            player.Walls.Add(this.Walls[i]);
        }
        player.Floor = this.Floor;
        player.Ceiling = this.Ceiling;
        player.RoomCenter = this.RoomCenter;
        Player.list.Remove(Id);
        Player.list.Add(player.Id, player);
    }

    private static IEnumerator LoadYourAsyncScene(ushort id, string destinationScene)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationScene, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene

        SceneManager.MoveGameObjectToScene(list[id].gameObject, SceneManager.GetSceneByName(destinationScene));
        SceneManager.MoveGameObjectToScene(NetworkManager.Singleton.gameObject, SceneManager.GetSceneByName(destinationScene));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
    #endregion
}
