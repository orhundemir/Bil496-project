using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Email { get; private set; }
    public string Uid { get; private set; }


    private void OnDestroy()
    {
        list.Remove(Id);
    }



    //User servera baðlandýðý bilgisi gelince server tarafýnda (Guest) Player gameobject oluþturuluyor.
    public static void Spawn(ushort id)
    {
        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} Guest";
        player.Id = id;
        list.Add(id, player);
    }

    //User google sign in baðlandýðý bilgisi gelince server tarafýnda daha önceden oluþan guest object update ediliyor.
    //ve client'a kendi player objectini oluþtursun diye SendSpawn mesajýný yolluyor.
    public static void Spawn(ushort id, string _email, string _uid)
    {
        foreach (Player player in list.Values)
        {
            if (player.Id == id)
            {
                player.name = $"Player {id} ({(string.IsNullOrEmpty(_email) ? "Guest" : _email)})";
                player.Id = id;
                player.Email = _email;
                player.Uid = _uid;

                //player bilgileri çekilsin diye DBManager'a gönderiliyor.
                DBManager.Singleton.playerList[id] = player;
                player.SendSpawned();
            }
        }
    }
    private static void ConnectToServerCheck(ushort id, bool _state)
    {
        if (_state)
        {
            Spawn(id);
        }
        else
        {
            Debug.Log("Guest Player " + id + "' in Server'a baðlanma giriþimi baþarýsýz oldu.");
        }
    }



    //Client email bilgisini gönderdiði zaman log gösteriliyor ve tutulan liste gelen client bilgileri güncelleniyor.
    public static void ShowEmailLog(ushort id, string _email)
    {
        Debug.Log("Email is: " + _email);
        list.GetValueOrDefault(id).Email = _email;
    }
    
    //Client uid bilgisini gönderdiði zaman log gösteriliyor ve tutulan liste gelen client bilgileri güncelleniyor.
    public static void ShowGoogleUidLog(ushort id, string _uid)
    {
        Debug.Log("UID is: " + _uid);
        list.GetValueOrDefault(id).Uid = _uid;
    }

    #region Messages
    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)));
    }

    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)), toClientId);
    }

    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Email);
        message.AddString(Uid);
        message.AddVector3(transform.position);
        return message;
    }

    [MessageHandler((ushort)ClientToServerId.connect)]
    private static void Connect(ushort fromClientId, Message message)
    {
        ConnectToServerCheck(fromClientId, message.GetBool());
    }
    [MessageHandler((ushort)ClientToServerId.googleEmail)]
    private static void GoogleEmail(ushort fromClientId, Message message)
    {
        ShowEmailLog(fromClientId, message.GetString());
    }
    
    [MessageHandler((ushort)ClientToServerId.googleUID)]
    private static void GoogleUID(ushort fromClientId, Message message)
    {
        ShowGoogleUidLog(fromClientId, message.GetString());
        Spawn(fromClientId, list.GetValueOrDefault(fromClientId).Email, list.GetValueOrDefault(fromClientId).Uid);
    }

    [MessageHandler((ushort)ClientToServerId.roomTemplate)]
    private static void RoomTemplate(ushort fromClientId, Message message)
    {
        int size = message.GetInt();

        for(int i = 0; i<size;i++)
        {
            Debug.Log(message.GetVector3().ToString());
            Debug.Log(message.GetQuaternion().ToString());
            Debug.Log(message.GetVector3().ToString());
        }
        
    }
    #endregion
}
