using Npgsql;
using Newtonsoft.Json;
public class UsersController{
    public bool insertUser(NpgsqlConnection conn, User user){//Insert given user with e_mail to database id is determined by last users'id+1 in database
        NpgsqlCommand command = conn.CreateCommand();
        string query = "INSERT INTO USERS (e_mail) VALUES ('"+user.e_mail+"')";
        command.CommandText = query;
        command.ExecuteNonQuery();
        ConnectionManager.closeConnection(conn);
        return true;
    }
    public User selectUser(NpgsqlConnection conn, string e_mail){//Returns user with given e_mail usefull for checking existance of user in database e.g. login
        User user = new User();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT e_mail FROM USERS WHERE e_mail = '"+e_mail+"'";
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
             user.e_mail = JsonConvert.SerializeObject(reader.GetValue(0));
        }
        ConnectionManager.closeConnection(conn);
        return user;
    }
    public User selectUser(NpgsqlConnection conn, int id){//Returns user with given id usefull for geting user from relational tables
        User user = new User();
        NpgsqlCommand command = conn.CreateCommand();
        string query = "SELECT e_mail FROM USERS WHERE id = "+id;
        command.CommandText = query;
        NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read()){
             user.e_mail = JsonConvert.SerializeObject(reader.GetValue(0));
        }
        ConnectionManager.closeConnection(conn);
        return user;
    }
}