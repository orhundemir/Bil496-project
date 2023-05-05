using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Email { get; private set; }
    public string Uid { get; private set; }

    public string roomName { get; private set; }

    private static User user;
    private static Room room;




    private void OnDestroy()
    {
        list.Remove(Id);
    }




    public static void Spawn(ushort id)
    {
        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} Guest";
        player.Id = id;
        player.Email = "";
        player.Uid = "";
        list.Add(id, player);
        player.SendSpawned(id);
    }


    /*
        * Google sign in basariliyla gerceklesirse user kaydi burada db'ye kaydediliyor.
    */
    public static void ShowEmailLog(ushort id, string _email)
    {
        Debug.Log("Email is: " + _email);
        list.GetValueOrDefault(id).Email = _email;
        list.GetValueOrDefault(id).name = _email;
        user = new User(id, _email);
        user = DBManager.checkUser(user);
    }

    public static void ShowGoogleUidLog(ushort id, string _uid)
    {
        Debug.Log("UID is: " + _uid);
        list.GetValueOrDefault(id).Uid = _uid;
    }

    public static void SaveRoomNameForNewToDB(ushort id, string roomName)
    {
        // TODO METIN:
        // KULLANICI YENI ODA OLUSTURDUGUNDA (KULLANICI, ODA) TABLOSUNA ID ve ODA ISMI INSERT EDILMELIDIR.
        // ***** ROOM NAME DBYE BU METOTDA EKLENMELI  *****
        room = new Room();
        room.name = roomName;
        list[id].roomName = roomName;
        Debug.Log("Room opened name: "+roomName);
    }
    
    public static void SaveRoomTemplateToDB(ushort id, Message message)
    {
       string wall = message.GetString();
       string products = message.GetString();
       room.wall = wall;
       room.ceiling = "";
       room.floor = "";
       room.furniture = products;
       DBManager.insertRoom(room, user);
       Debug.Log("Kullanıcının odası basariyla db ye yuklendi");
    }





    #region SendMessages
    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerSpawned)));
    }

    public void SendSpawned(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerSpawned)), toClientId);
    }

    private static void  SendRoomNames(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddRoomNamesData(Message.Create(MessageSendMode.Reliable, ServerToClientId.roomNames)), toClientId);
    }
    private static void SendRoomTemplate(ushort toClientId,string roomName)
    {
        NetworkManager.Singleton.Server.Send(AddRoomTemplateData(Message.Create(MessageSendMode.Reliable, ServerToClientId.roomTemplate),roomName), toClientId);
    }



    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Email);
        message.AddString(Uid);
        message.AddVector3(transform.position);
        return message;
    }


    private static Message AddRoomNamesData(Message message)
    {

        List<Room> list =DBManager.myRooms(user);
        int count = list.Count;
        message.AddInt(count);
        for(int i =0;i<count;i++)
        {
            string roomName = list[i].name;
            message.AddString(roomName);
        }


        return message;
    }

    private static Message AddRoomTemplateData(Message message,string roomName)
    {
        //TODO METIN:
        // DB DEN KULLANICININ SEÇTIGI ODA SEÇİLECEK (kullanicinin sectigi oda roomName'dir)
        // SONRA BU ODAYA DAIR WALL VE PRODUCKTS STRINGLERI ASAGIDAKI SEKILDE ASSIGN EDILMELI

        string wall = "wall stringi dbden cekilecek";
        string product = "product stringi dbden cekilecek";
        message.AddString(wall);
        message.AddString(product);

        return message;
    }



    #endregion


    #region Messages
    [MessageHandler((ushort)ClientToServerId.connected)]
    private static void Connected(ushort fromClientId, Message message)
    {
        Debug.Log("ACK message is: "+message.GetBool());
    }

    [MessageHandler((ushort)ClientToServerId.googleEmail)]
    private static void GoogleEmail(ushort fromClientId, Message message)
    {
        string email = message.GetString();
        ShowEmailLog(fromClientId, email);
    }
    
    [MessageHandler((ushort)ClientToServerId.googleUID)]
    private static void GoogleUID(ushort fromClientId, Message message)
    {
        ShowGoogleUidLog(fromClientId, message.GetString());
        SendRoomNames(fromClientId); // Google Sign in olunca db de kayıtlı room isimleri de gonderilir.
    }
    [MessageHandler((ushort)ClientToServerId.roomName)]
    private static void RoomName(ushort fromClientId, Message message)
    {
        string roomName = message.GetString();
        list[fromClientId].roomName = roomName;
        Debug.Log("Client şu odayı yeni oluşturdu: " + roomName);
        SaveRoomNameForNewToDB(fromClientId, roomName);
    }

    [MessageHandler((ushort)ClientToServerId.prevRoomName)]
    private static void PrevRoomName(ushort fromClientId, Message message)
    {
        string roomName = message.GetString();
        list[fromClientId].roomName = roomName;
        Debug.Log("Client şu odayı yuklemek istedi" + roomName );
        SendRoomTemplate(fromClientId,roomName);
    }

    [MessageHandler((ushort)ClientToServerId.roomTemplate)]
    private static void RoomTemplate(ushort fromClientId, Message message)
    {
        Debug.Log("Client odasını kaydetti. Bu oda DBye yuklenmelidir.");
        SaveRoomTemplateToDB(fromClientId,message);
    }
    #endregion
}
