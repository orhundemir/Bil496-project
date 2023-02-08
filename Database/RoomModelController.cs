using Npgsql;
using Newtonsoft.Json;
using System.Collections.Generic;

public class RoomModelController{

    public bool insertRoomModel(NpgsqlConnection conn, RoomModel roomModel){//Inserting given room model to database id will be determined by last room model's id+1 in database
        NpgsqlCommand command = conn.CreateCommand();
        string query = "INSERT INTO ROOM_MODEL (scene_id, model_id, x, y, z) VALUES ("+roomModel.scene_id+","+roomModel.model_id+","+roomModel.x+","+roomModel.y+","+roomModel.z+")";
        command.CommandText = query;
        command.ExecuteNonQuery();
        ConnectionManager.closeConnection(conn);
        return true;
    }
    public List<RoomModel> selectRoomsModels(NpgsqlConnection conn, Room room){//Selecting a users room model in database
        List<RoomModel> roomModels = new List<RoomModel>();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM ROOM_MODEL WHERE scene_id = "+room.id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            RoomModel roomModel = new RoomModel();
            roomModel.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            roomModel.scene_id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
            roomModel.model_id = int.Parse((JsonConvert.SerializeObject(reader.GetValue(2))));
            roomModel.x = int.Parse((JsonConvert.SerializeObject(reader.GetValue(3))));
            roomModel.y = int.Parse((JsonConvert.SerializeObject(reader.GetValue(4))));
            roomModel.z = int.Parse((JsonConvert.SerializeObject(reader.GetValue(5))));
            roomModels.Add(roomModel);
        }
        return roomModels;
    }
}