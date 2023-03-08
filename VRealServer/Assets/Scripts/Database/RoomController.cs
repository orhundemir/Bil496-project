using Npgsql;
using Newtonsoft.Json;
public class RoomController{
    
    public bool insertRoom(NpgsqlConnection conn, Room room){//Inserting given room to database id will be determined by last room's id+1 in database
        NpgsqlCommand command = conn.CreateCommand();
        string query = "INSERT INTO Rooms (name) VALUES ('"+room.name+"')";
        command.CommandText = query;
        command.ExecuteNonQuery();
        ConnectionManager.closeConnection(conn);
        return true;
    }
    public Room selectRoom(NpgsqlConnection conn, int id){//Returning room with given id usefull for getting room from relational tables
        Room room = new Room();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM USERS WHERE id = "+id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            room.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            room.name = JsonConvert.SerializeObject(reader.GetValue(1));
        }
        ConnectionManager.closeConnection(conn);
        return room;
    }
    //Name won't be unique so getting room from names will be manually done by user from relational tables
}