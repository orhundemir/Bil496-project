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
        string wallString = message.GetString();
        string productString = message.GetString();

        //string wallString = "2-_-1,658282-_-1,124998-_-5,365653-_-0-_-86,9529-_-0-_-0,66-_-2,25-_-1,5-_-Door Material 1 (Instance) (Instance)-_-0-_-0-_-0***0-_--10,16685-_-1,499998-_-4,736178-_-0-_-86,9529-_-0-_-0,6000001-_-3-_-14,99254-_-Opaque Wall (Instance) (Instance)-_-0,09012479-_-0,9555748-_-0***1-_-4,804496-_-1,499998-_-1,662215-_-0-_-180-_-0-_-0,66-_-1,8-_-1,8-_-WindowMaterial (Instance) (Instance)-_-1-_-1-_-1***1-_-4,804495-_-1,499998-_--1,127123-_-0-_-180-_-0-_-0,66-_-1,8-_-1,8-_-WindowMaterial (Instance) (Instance)-_-1-_-1-_-1***1-_-4,804495-_-1,499998-_-0,3529339-_-0-_-180-_-0-_-0,66-_-1,8-_-1,8-_-WindowMaterial (Instance) (Instance)-_-1-_-1-_-1***0-_-4,804498-_-1,499998-_-5,533133-_-0-_-180-_-0-_-0,6-_-3-_-10,01885-_-Opaque Wall (Instance) (Instance)-_-0,09012479-_-0,9555748-_-0***2-_--4,34191-_-1,124998-_--6,233692-_-0-_-259,1806-_-0-_-0,66-_-2,25-_-1,5-_-Door Material 1 (Instance) (Instance)-_-0-_-0-_-0***2-_-3,138361-_-1,124998-_--4,804129-_-0-_-259,1806-_-0-_-0,66-_-2,25-_-1,5-_-Door Material 1 (Instance) (Instance)-_-0-_-0-_-0***0-_-4,804493-_-1,499998-_--4,485713-_-0-_-259,1806-_-0-_-0,6-_-3-_-13,03999-_-Wall3 (Instance) (Instance)-_-1-_-1-_-1***1-_--11,64969-_-1,499998-_--4,670465-_-0-_-301,8275-_-0-_-0,66-_-1,8-_-1,8-_-WindowMaterial (Instance) (Instance)-_-1-_-1-_-1***0-_--8,003695-_-1,499998-_--6,933499-_-0-_-301,8275-_-0-_-0,6-_-3-_-11,65787-_-Opaque Wall (Instance) (Instance)-_-0,2509804-_-0,2627451-_-0,3058823***0-_--17,90869-_-1,499998-_--0,7855716-_-0-_-90,49392-_-0-_-0,6000001-_-3-_-6,603579-_-Planks5c (Instance) (Instance)-_-1-_-1-_-1***1-_--10,75636-_-1,499998-_-1,847581-_-0-_-11,5346-_-0-_-0,6600001-_-1,8-_-1,8-_-WindowMaterial (Instance) (Instance)-_-1-_-1-_-1***0-_--11,30536-_-1,499998-_--0,8424969-_-0-_-11,5346-_-0-_-0,6000001-_-3-_-5,693664-_-Wall3 (Instance) (Instance)-_-1-_-1-_-1***3-_--6,552095-_-3,15-_--0,7001834-_-0-_-0-_-0-_-22,71318-_-0,3-_-12,46663-_-Ceiling Material (Instance) (Instance)-_-0-_-0-_-0***4-_--6,552095-_--0,15-_--0,7001834-_-0-_-0-_-0-_-22,71318-_-0,3-_-12,46663-_-Planks5a (Instance) (Instance)-_-0-_-0-_-0***";
        //string productString = "-7,890553-_--0,08684979-_--3,152982-_-2,229584E-06-_-328,3836-_--1,742997E-06-_-0,002-_-0,002-_-0,002-_-alex-drawer-unit-black-brown-60473548(Clone)***-2,536808-_--0,08585257-_--2,996163-_-88,47069-_-310,5151-_-42,97285-_-0,002-_-0,002-_-0,002-_-karmsund-table-mirror-black-60507261(Clone)***-9,108209-_-0,3885658-_-1,884027-_--3,415095E-06-_-283,4099-_-90-_-0,002-_-0,002-_-0,002-_-ingolf-chair-white-70103250(Clone)***";

        string[] walls = wallString.Replace("\"", "").Split("***");
        string[] products = productString.Replace("\"", "").Split("***");

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
            string path = "RoomDrawer/" + wallData[10].Replace("(Instance)", "").Trim();
            Material material = Resources.Load<Material>(path);
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

        foreach (string product in products)
        {
            string[] productData = product.Split("-_-");
            if (productData[0].Equals(""))
                continue;

            Vector3 position = new Vector3(float.Parse(productData[0]), float.Parse(productData[1]), float.Parse(productData[2]));
            Vector3 rotation = new Vector3(float.Parse(productData[3]), float.Parse(productData[4]), float.Parse(productData[5]));
            Vector3 scale = new Vector3(float.Parse(productData[6]), float.Parse(productData[7]), float.Parse(productData[8]));
            string name = productData[9].Replace("(Clone)", "").Trim();

            string prefabPath = "Furnitures/" + name;
            GameObject productObject = Resources.Load<GameObject>(prefabPath);

            productObject.transform.position = position;
            productObject.transform.eulerAngles = rotation;
            productObject.transform.localScale = scale;
            productObject.name = name;

            list[id].Products.Add(productObject);
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
