using Npgsql;
using Newtonsoft.Json;
public class UsersController{
    public int insertUser(NpgsqlConnection conn, User user){//Insert given user with e_mail to database id is determined by last users'id+1 in database
        conn.Open();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "INSERT INTO USERS (e_mail) VALUES ('"+user.e_mail+"') RETURNING id";
        command.CommandText = query;
        int id = command.ExecuteNonQuery();
        conn.Close();
        return id;
    }
    public User selectUser(NpgsqlConnection conn, string e_mail){//Returns user with given e_mail usefull for checking existance of user in database e.g. login
        conn.Open();
        User user = new User();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT e_mail, id FROM USERS WHERE e_mail = '"+e_mail+"'";
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
             user.e_mail = JsonConvert.SerializeObject(reader.GetValue(0));
             user.id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
        }
        conn.Close();
        return user;
    }
    public User selectUser(NpgsqlConnection conn, int id){//Returns user with given id usefull for geting user from relational tables
        conn.Open();
        User user = new User();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT e_mail, id FROM USERS WHERE e_mail = '"+id+"'";
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
             user.e_mail = JsonConvert.SerializeObject(reader.GetValue(0));
             user.id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(1)));
        }
        conn.Close();
        return user;
    }
    public int getLastUser(NpgsqlConnection conn){
        conn.Open();
        int id = 0;
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT id FROM Users ORDER BY id DESC LIMIT 1";
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
             id = int.Parse(JsonConvert.SerializeObject(reader.GetValue(0)));
        }
        conn.Close();
        return id;
    }
}