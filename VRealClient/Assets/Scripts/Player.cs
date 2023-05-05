using HTC.UnityPlugin.Vive;
using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public List<string> RoomsNames { get; set; }
    public Vector3 RoomCenter { get; set; }

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    // Vr sahnesinde duvar objelerinin yaratýlmasýný saðlýyor.
    public void SpawnGameObjects(GameObject wall, Transform parentObject)
    {
        GameObject wallObject = Instantiate(wall, wall.transform.position, wall.transform.rotation);
        wallObject.transform.parent = parentObject;
    }

    // Client basarili þekilde Sign in oldu. Player spawn oluyor.
    // Þimdilik basit bir capsule oluþuyor ihtiyaca göre deðiþebilir.
    public static void Spawn(ushort id, string _email, string _uid, Vector3 position)
    {
        position.x = -5.63f;
        Player player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
        player.Id = id;
        player.name = _email;
        player.Email = _email;
        player.Uid = _uid;
        player.RoomsNames = new List<string>();
        player.Walls = new List<GameObject>();
        player.Products = new List<GameObject>();
        list[id] = player;
        player.gameObject.SetActive(false);
    }


    public static void AssingRoomObjectsToPlayer(Message message)
    {
        ushort id = NetworkManager.Singleton.Client.Id;
        // string wallString = message.GetString();
        // string productString = message.GetString();
        string wallString = "2-_-1,811825-_-1,124998-_-7,744446-_-5,830823E-06-_-89,44193-_-2,035555E-13-_-0,66-_-2,25-_-1,5-_-Door Material 1 (Instance)-_-0-_-0-_-0***2-_--3,661079-_-1,124998-_-7,69114-_-5,830823E-06-_-89,44193-_-2,035555E-13-_-0,66-_-2,25-_-1,5-_-Door Material 1 (Instance)-_-0-_-0-_-0***0-_--13,64228-_-1,499998-_-7,593919-_-5,830823E-06-_-89,44193-_-2,035555E-13-_-0,6-_-3-_-18,7423-_-Wall1 (Instance)-_-1-_-1-_-1***0-_-5,099125-_-1,499996-_-7,776466-_--1,32857E-05-_-177,0318-_-2,544444E-14-_-0,6-_-3-_-16,45123-_-Wall1 (Instance)-_-1-_-1-_-1***1-_--6,042444-_-1,5-_--8,384466-_-0-_-271,2812-_-0-_-0,66-_-1,8-_-1,8-_-WindowMaterial (Instance)-_-1-_-1-_-1***0-_-5,951007-_-1,5-_--8,652689-_-0-_-271,2812-_-0-_-0,6-_-3-_-19,05042-_-Opaque Wall (Instance)-_-1-_-0-_-0***1-_--11,70067-_-1,5-_--3,798823-_-1,586197E-05-_-17,47479-_-0-_-0,66-_-1,8-_-1,8-_-WindowMaterial (Instance)-_-1-_-1-_-1***0-_--13,09465-_-1,5-_--8,226751-_-1,586197E-05-_-17,47479-_-0-_-0,6-_-3-_-6,889626-_-Opaque Wall (Instance)-_-0,2509804-_-0,2627451-_-0,3058823***0-_--11,02579-_-1,499998-_--1,655087-_-0-_-262,875-_-0-_-0,6-_-3-_-7,849252-_-Opaque Wall (Instance)-_-0,4754791-_-0,8467359-_-0***0-_--18,81443-_-1,499998-_--2,628666-_-0-_-358,4089-_-0-_-0,6000001-_-3-_-4,3828-_-Opaque Wall (Instance)-_-0,2509804-_-0,2627451-_-0,3058823***0-_--18,93613-_-1,499998-_-1,752443-_-0-_-86,05482-_-0-_-0,6-_-3-_-7,075221-_-Opaque Wall (Instance)-_-0,4754791-_-0,8467359-_-0***0-_--11,87767-_-1,499998-_-2,239231-_-0-_-341,7606-_-0-_-0,6-_-3-_-5,637956-_-Opaque Wall (Instance)-_-0,2509804-_-0,2627451-_-0,3058823***3-_--6,49256-_-3,15-_--0,4381115-_-0-_-0-_-0-_-24,88713-_-0,3-_-16,42916-_-Ceiling Material (Instance)-_-0-_-0-_-0***4-_--6,49256-_--0,15-_--0,4381115-_-0-_-0-_-0-_-24,88713-_-0,3-_-16,42916-_-Floor2 (Instance) (Instance)-_-0-_-0-_-0***";
        string productString = "-4,114692-_--0,08685063-_-5,339195-_--3,235102E-05-_-10,49509-_--0,0009581303-_-0,002-_-0,002-_-0,002-_-alex-drawer-unit-black-brown-60473548(Clone)***-4,542412-_--0,09788189-_-4,457348-_-291,4928-_-311,3912-_-96,55825-_-0,02-_-0,02-_-0,02-_-alex-drawer-unit-on-casters-white-80485423(Clone)***-4,557808-_--0,09804774-_-4,446943-_-336,5148-_-0,07229744-_-14,11501-_-0,02-_-0,02-_-0,02-_-hemnes-3-drawer-chest-white-stain-70360414(Clone)***";

        string[] walls = wallString.Split("***");
        string[] products = productString.Split("***");

        foreach (string wall in walls)
        {
            string[] wallData = wall.Split("-_-");
            if (wallData[0].Equals(""))
                continue;

            // Parse the received data
            int type = int.Parse(wallData[0]);
            Vector3 position = new Vector3(float.Parse(wallData[1]), float.Parse(wallData[2]), float.Parse(wallData[3]));
            Vector3 rotation = new Vector3(float.Parse(wallData[4]), float.Parse(wallData[5]), float.Parse(wallData[6]));
            Vector3 scale = new Vector3(float.Parse(wallData[7]), float.Parse(wallData[8]), float.Parse(wallData[9]));
            string path = "Assets/Materials/RoomDrawer/" + wallData[10].Replace("(Instance)", "").Trim() + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            Color color = new Color(float.Parse(wallData[11]), float.Parse(wallData[12]), float.Parse(wallData[13]));

            // Create the cube object that contains all the necessary components
            GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            MeshRenderer meshRenderer = cubeObject.GetComponent<MeshRenderer>();
            meshRenderer.material = material;
            meshRenderer.material.color = color;
            cubeObject.AddComponent<BoxCollider>();
            Rigidbody rigidBody = cubeObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;

            // Assign the object's tag
            if (type == 0) cubeObject.tag = "Wall";
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
                cubeObject.transform.localScale = Vector3.one;
                cubeObject.transform.localRotation = Quaternion.identity;
                if (type == 0)
                    cubeObject.transform.position = cubeObject.transform.parent.TransformPoint(new Vector3(0, 0, 0.5f));
                else
                    cubeObject.transform.position = cubeObject.transform.parent.TransformPoint(Vector3.zero);

                list[id].Walls.Add(parentObject);
            }
            // Create the ceiling/floor objects
            else if (type == 3 || type == 4)
            {
                cubeObject.transform.position = position;
                cubeObject.transform.eulerAngles = rotation;
                cubeObject.transform.localScale = scale;
                cubeObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);

                // Assign the object to the current Player's corresponding field
                if (type == 3)
                {
                    list[id].Ceiling = cubeObject;
                    list[id].RoomCenter = cubeObject.transform.position + new Vector3(0, -1, 0);
                }
                else if (type == 4)
                {
                    cubeObject.AddComponent<Teleportable>();
                    list[id].Floor = cubeObject;
                }
            }
        }

        list[id].gameObject.SetActive(true);
        MovePlayerToDestinationScene(id, "VReal");
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
        int count = message.GetInt();
        for(int i=0;i<count;i++)
        {
            list[NetworkManager.Singleton.Client.Id].RoomsNames.Add(message.GetString());
        }
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
        player.name = Email;
        player.Id = NetworkManager.Singleton.Client.Id;
        player.Email = Email;
        player.Uid = Uid;
        player.RoomName = RoomName;

        int size = Walls.Count;
        player.Walls = new List<GameObject>(size);
        for (int i = 0; i < size; i++)
        {
            player.Walls.Add(Walls[i]);
        }

        size = Products.Count;
        player.Products = new List<GameObject>(size);
        for (int i = 0; i < size; i++)
        {
            player.Products.Add(Products[i]);
        }

        size = RoomsNames.Count;
        player.RoomsNames = new List<string>(size);
        for (int i = 0; i < size; i++)
        {
            player.RoomsNames.Add(RoomsNames[i]);
        }

        player.Floor = Floor;
        player.Ceiling = Ceiling;
        player.RoomCenter = RoomCenter;
        list.Remove(Id);
        list.Add(player.Id, player);
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
