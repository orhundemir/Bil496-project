using Npgsql;
using Newtonsoft.Json;
public class RoomController{
    
    public int insertRoom(NpgsqlConnection conn, Room room){//Inserting given room to database id will be determined by last room's id+1 in database
        conn.Open();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "INSERT INTO Rooms (name, wall, ceiling, floor, furniture) VALUES ('"+room.name+"', '"+room.wall+"', '"+room.ceiling+"', '"+room.floor+"', '"+room.furniture+"') RETURNING id";
        command.CommandText = query;
        int id = command.ExecuteNonQuery();
        conn.Close();
        return id;
    }
    public void updateRoom(NpgsqlConnection conn, Room room){//Updating existing room
        conn.Open();
        NpgsqlCommand command = conn.CreateCommand();
        string query ="UPDATE ROOMS SET furniture = \'"+room.furniture+"\' WHERE id = "+room.id;
        command.CommandText = query;
        conn.Close();
    }

    public Room selectRoom(NpgsqlConnection conn, int id){//Returning room with given id usefull for getting room from relational tables
        conn.Open();
        Room room = new Room();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM Rooms WHERE id = "+id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            room.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            room.name = JsonConvert.SerializeObject(reader.GetValue(1));
            room.wall = JsonConvert.SerializeObject(reader.GetValue(2));
            room.ceiling = JsonConvert.SerializeObject(reader.GetValue(3));
            room.floor = JsonConvert.SerializeObject(reader.GetValue(4));
            room.furniture = JsonConvert.SerializeObject(reader.GetValue(5));
        }
        conn.Close();
        return room;
    }
    public Room getRoomName(NpgsqlConnection conn, int id){//Returning room's name with given id usefull for getting room from relational tables
        conn.Open();
        Room room = new Room();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT name FROM Rooms WHERE id = "+id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            room.id  = id;
            room.name = JsonConvert.SerializeObject(reader.GetValue(0));
        }
        conn.Close();
        return room;
    }
    public Room selectRoomWithName(NpgsqlConnection conn, string name){//Returning room with given name
        conn.Open();
        Room room = new Room();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT * FROM Rooms WHERE name = '"+name+"'";
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
            room.id  = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
            room.name = JsonConvert.SerializeObject(reader.GetValue(1));
            room.wall = JsonConvert.SerializeObject(reader.GetValue(2));
            room.ceiling = JsonConvert.SerializeObject(reader.GetValue(3));
            room.floor = JsonConvert.SerializeObject(reader.GetValue(4));
            room.furniture = JsonConvert.SerializeObject(reader.GetValue(5));
        }
        conn.Close();
        return room;
    }
    public int getLastRoomId(NpgsqlConnection conn){
        conn.Open();
        int id = 0;
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT id FROM Rooms ORDER BY id DESC LIMIT 1";
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
             id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
        }
        conn.Close();
        return id;
    }
}