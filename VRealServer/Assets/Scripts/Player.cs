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
   * Google sign in basariliyla gerceklesirse user kaydi db'ye kaydediliyor.
   */
    public static void DBUserRecord(ushort id, string _email)
    {
        foreach (Player player in list.Values)
        {
            if (player.Id == id)
            {
                user = new User(id, _email);
                user = DBManager.checkUser(user);
            }
        }
    }



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

    public static void RoomNameForNew(ushort id, string roomName)
    {
        /*
         Client yeni oluşturduğu room nameyi kendi player instancı olan room nameyi kaydetti. Ve bu room nameyi Servera gönderdi.
         Burası serverın bu paketi aldığı ilk metot olarak düşünülebilir.

         Burada roomname unique ise database e kaydedilecek unique değilse 
         sonuna bir id no gibi bir şey eklenip kaydedebilir.
         Bu metot sadece yukarda anlatılanları yapacak.

         
         Başka bir metotda
         Client bu ilk olusturdugu odayı kaydettiğinde ve başka bir zamanda bu odayı düzenlemek istediğinde
         Client bu odanın ismini tekrar gonderecek(Load) ve server bu odanın ismiyle 
         odanın duvarlarını,zeminini, tavanını ve mobilya objelerini (roomTemplate)
         Clienta iletecek. Clientta bu objeleri Player sınıfının ilgili instancelarına atayacak.
        
         */
        // ***** ROOM NAME DBYE BURADA EKLENMELI         *****
         room = new Room();
         room.name = roomName;
         list[id].roomName = roomName;
         Debug.Log("Room opened name: "+roomName);
    }
    
    public static void SaveRoomTemplateToDB(ushort id, Message message)
    {
        
       // Vreal sahnesinde oda kaydetme geliştirmesi bittikten sonra client odayı kaydedince odanın objelerinin servera iletdigi ilk yer burası.
        //Bu metotda gelen oda template okunup json,binary vs. formatına çevrildikten sonra db'ye kaydedilmelidir.

       // Bu metot şimdilik sadece oda çizim sahnesinde oda çizildikten duvarların konumlarının serverda ekrana basma işini yapıyor.

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
        //NetworkManager.Singleton.Server.Send(AddRoomNamesData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)), toClientId);
    }
    private static void SendRoomTemplate(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddRoomTemplateData(Message.Create(MessageSendMode.Reliable, ServerToClientId.playerSpawned)), toClientId);
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
        /*
         Client daha önce oluşturduğu odaları görüntülemek istediğine dair mesajı servera gönderdi. 
         Bu görüntülemek istediği odaların isimleri dbden çekilip bu metodun message parametresine eklenmelidir.
         Bu oda isimleri  SendRoomNames metoduyla ile clienta iletilecektir.

         */
        return message;
    }

    private static Message AddRoomTemplateData(Message message)
    {
        /*
         Client daha önce oluşturduğu odayı yuklemek istediğine dair mesajı servera gönderdi. (Bu mesaj google sign in yapıldıktan sonra gonderiliyor.)
         Bu tekrar yuklemek istediği odanın objeleri dbden çekilip bu metodun message parametresine eklenmelidir.
         
         Bu oda objeleri  SendRoomTemplate metoduyla ile clienta iletilecektir.

         Onemli: Eger mesaja eklenen türler farklı ise mesaja ilk olarak kaç tane obje gonderileceğinin
         bilgisi verilmelidir. Ornek message.AddInt(10)
         */
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
        DBUserRecord(fromClientId, email);
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
        Debug.Log("Client şu odayı yeni oluşturdu: " + roomName);
        RoomNameForNew(fromClientId, roomName);
    }

    [MessageHandler((ushort)ClientToServerId.prevRoomName)]
    private static void PrevRoomName(ushort fromClientId, Message message)
    {
        Debug.Log("Client şu odayı yuklemek istedi" + message.GetString());
        SendRoomTemplate(fromClientId);
    }

    [MessageHandler((ushort)ClientToServerId.roomTemplate)]
    private static void RoomTemplate(ushort fromClientId, Message message)
    {
        Debug.Log("Client odasını kaydetti. Bu oda DBye yuklenmelidir.");
        SaveRoomTemplateToDB(fromClientId,message);
    }
    #endregion
}
