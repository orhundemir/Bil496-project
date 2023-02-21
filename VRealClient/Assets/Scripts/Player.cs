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
    public string NameAndSurname { get; private set; }
    public string Uid { get; private set; }

    public GameObject Walls { get; private set; }


    private void OnDestroy()
    {
        list.Remove(Id);
    }


    // Client basarili þekilde Sign in oldu. Player spawn oluyor.
    // Þimdilik basit bir capsule oluþuyor ihtiyaca göre deðiþebilir.
    public static void Spawn(ushort id, string _email, string _nameAndSurname, string _uid, Vector3 position)
    {
        GameObject go = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity);
        Player player  = go.GetComponent<Player>();
        player.name = _email;
        player.Id = id;
        player.Email = _email;
        player.NameAndSurname = _nameAndSurname;
        player.Uid = _uid;
        list.Add(id, player);


        // Aciklama: Oyuncu Google Sign'ini gerçekleþtirdi ve spawn olmaya hazýr hale geldi.
        // Sahne deðiþimi gerekli.
        MovePlayerToDestinationScene(id, "RoomDrawing");
    }

   

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetString(), message.GetString(), message.GetVector3());
        Debug.Log("Client connected to server successfully");
    }



    #region ScenePassingProcess
    private static void MovePlayerToDestinationScene(ushort id, string destinationScene)
    {
        list[id].StartCoroutine(LoadYourAsyncScene(id, destinationScene));
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
