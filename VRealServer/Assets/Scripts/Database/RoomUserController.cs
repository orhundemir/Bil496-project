using Npgsql;
using Newtonsoft.Json;
using System.Collections.Generic;

public class RoomUserController{
    public bool insertRoomUser(NpgsqlConnection conn, RoomUser roomUser){//Inserting given room user to database id will be determined by last room user's id+1 in database
        conn.Open();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "INSERT INTO ROOM_USERS (user_id, scene_id) VALUES ("+roomUser.user_id+","+roomUser.scene_id+")";
        command.CommandText = query;
        command.ExecuteNonQuery();
        conn.Close();
        return true;
    }
    public List<RoomUser> selectRoomsUser(NpgsqlConnection conn, Room room){//Selecting users from database which can access gien room
        conn.Open();
        List<RoomUser> roomUsers = new List<RoomUser>();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM ROOM_USERS WHERE scene_id = "+room.id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            RoomUser roomUser = new RoomUser();
            roomUser.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            roomUser.user_id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
            roomUser.scene_id = int.Parse((JsonConvert.SerializeObject(reader.GetValue(2))));
            roomUsers.Add(roomUser);
        }
        conn.Close();
        return roomUsers;
    }
    public List<RoomUser> selectUsersRoom(NpgsqlConnection conn, User user){//Selecting users rooms from database
        conn.Open();
        List<RoomUser> roomUsers = new List<RoomUser>();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM ROOM_USERS WHERE user_id = "+user.id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            RoomUser roomUser = new RoomUser();
            roomUser.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            roomUser.user_id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
            roomUser.scene_id = int.Parse((JsonConvert.SerializeObject(reader.GetValue(2))));
            roomUsers.Add(roomUser);
        }
        conn.Close();
        return roomUsers;
    }

}