using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Email { get; private set; }
    public string NameAndSurname { get; private set; }
    public string Uid { get; private set; }


    private void OnDestroy()
    {
        list.Remove(Id);
    }



    //User servera ba�land��� bilgisi gelince server taraf�nda (Guest) Player gameobject olu�turuluyor.
    public static void Spawn(ushort id)
    {
        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} Guest";
        player.Id = id;
        list.Add(id, player);
    }
    private static void ConnectToServerCheck(ushort id, bool _state)
    {
        if (_state)
        {
            Spawn(id);
        }
        else
        {
            Debug.Log("Guest Player " + id + "' in Server'a ba�lanma giri�imi ba�ar�s�z oldu.");
        }
    }



    //Client email bilgisini g�nderdi�i zaman log g�steriliyor ve tutulan liste gelen client bilgileri g�ncelleniyor.
    public static void ShowEmailLog(ushort id, string _email)
    {
        Debug.Log("Email is: " + _email);
        list.GetValueOrDefault(id).Email = _email;
    }
    //Client isim bilgisini g�nderdi�i zaman log g�steriliyor ve tutulan liste gelen client bilgileri g�ncelleniyor.
    public static void ShowNameSurnameLog(ushort id, string _nameSurname)
    {
        Debug.Log("Name and Surname: " + _nameSurname);
        list.GetValueOrDefault(id).NameAndSurname = _nameSurname;
    }
    //Client uid bilgisini g�nderdi�i zaman log g�steriliyor ve tutulan liste gelen client bilgileri g�ncelleniyor.
    public static void ShowGoogleUidLog(ushort id, string _uid)
    {
        Debug.Log("UID is: " + _uid);
        list.GetValueOrDefault(id).Uid = _uid;
    }



    //User google sign in ba�land��� bilgisi gelince server taraf�nda daha �nceden olu�an guest object update ediliyor.
    //ve client'a kendi player objectini olu�tursun diye SendSpawn mesaj�n� yolluyor.
    public static void Spawn(ushort id, string _email, string _nameAndSurname, string _uid)
    {
        foreach (Player player in list.Values)
        {
            if (player.Id == id)
            {
                player.name = $"Player {id} ({(string.IsNullOrEmpty(_email) ? "Guest" : _email)})";
                player.Id = id;
                player.Email = _email;
                player.NameAndSurname = _nameAndSurname;
                player.Uid = _uid;

                //player bilgileri �ekilsin diye DBManager'a g�nderiliyor.
                DBManager.Singleton.playerList[id] = player;

                User user = new User(id, _email);

                user = DBManager.checkUser(user);
                user.name = _nameAndSurname;
                
                player.SendSpawned();
            }
        }
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
        message.AddString(NameAndSurname);
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
    [MessageHandler((ushort)ClientToServerId.googleNameSurname)]
    private static void GoogleNameSurname(ushort fromClientId, Message message)
    {
        ShowNameSurnameLog(fromClientId, message.GetString());
    }
    [MessageHandler((ushort)ClientToServerId.googleUID)]
    private static void GoogleUID(ushort fromClientId, Message message)
    {
        ShowGoogleUidLog(fromClientId, message.GetString());
        Spawn(fromClientId, list.GetValueOrDefault(fromClientId).Email, list.GetValueOrDefault(fromClientId).NameAndSurname, list.GetValueOrDefault(fromClientId).Uid);
    }

    [MessageHandler((ushort)ClientToServerId.roomTemplate)]
    private static void RoomTemplate(ushort fromClientId, Message message)
    {
        int size = message.GetInt();

        for(int i = 0; i<size;i++)
        {
            Debug.Log(message.GetVector3().ToString());
            Debug.Log(message.GetQuaternion().ToString());
        }
        
    }
    #endregion
}
