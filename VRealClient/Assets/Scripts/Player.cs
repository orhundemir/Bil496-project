using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Email { get; private set; }
    public string Uid { get; private set; }
    public GameObject[] Walls { get; set; }
    public GameObject Floor { get; set; }  
    public GameObject Ceiling { get; set; }
    public Vector3 RoomCenter { get; set; }



    private void OnDestroy()
    {
        list.Remove(Id);
    }

    // Vr sahnesinde duvar objelerinin yarat�lmas�n� sa�l�yor.
    public void SpawnWalls(GameObject wall)
    {
        Instantiate(wall, wall.transform.position, wall.transform.rotation);
    }

    // Oyuncu objesini VR gozluge uyumlu objeye ceviriyor
    // Ya da vr sahnesinden cizim ekranina gecmek icin kullanilabilir
    public void CopyPlayer(Player player)
    {
        player.name = this.Email;
        player.Id = NetworkManager.Singleton.Client.Id;
        player.Email = this.Email;
        player.Uid = this.Uid;

        int size = this.Walls.Length;
        player.Walls = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            player.Walls[i] = this.Walls[i];
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

    // Client basarili �ekilde Sign in oldu. Player spawn oluyor.
    // �imdilik basit bir capsule olu�uyor ihtiyaca g�re de�i�ebilir.
    public static void Spawn(ushort id, string _email, string _uid, Vector3 position)
    {
        position.x = -5.63f;
        position.y = 0.8f;
        GameObject go = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity);
        Player player  = go.GetComponent<Player>();
        player.name = _email;
        player.Id = id;
        player.Email = _email;
        player.Uid = _uid;
        list.Add(id, player);
        go.SetActive(false);
    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetString(), message.GetVector3());
        Debug.Log("Client connected to server successfully");
    }


    #region ScenePassingProcess
    public static void MovePlayerToDestinationScene(ushort id, string destinationScene)
    {
        list[id].StartCoroutine(LoadYourAsyncScene(id, destinationScene));
    }
    #endregion
}
