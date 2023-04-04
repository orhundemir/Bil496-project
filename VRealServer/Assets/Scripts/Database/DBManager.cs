using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;

public class DBManager : MonoBehaviour
{
    private static DBManager _singleton;
	private static NpgsqlConnection conn;
    public static DBManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(DBManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    

    private void Awake()
    {
        Singleton = this;
    }
	
	//Public NpgsqlConnection object set within the parameters inside the ConnectionManager class
    private void setConnection(){
        conn = ConnectionManager.getConnection();
    }
    //Returns NpgsqlConnection object
    public static NpgsqlConnection getConnection(){
        return conn;
    }
    //Opens connection to the database
    public void openConnection(){
        conn.Open();
    }
    //Closes connection to the database
    public void closeConnection(){
        conn.Close();
    }
	
	public static User checkUser(User user){
        UsersController uc = new UsersController();
        User temp = uc.selectUser(conn, user.e_mail);
        if(temp.e_mail == null){
            Debug.Log("User: "+user.e_mail+" not exist adding new user to database");
            uc.insertUser(conn, user);
            temp = checkUser(temp);
        }
        else {
            Debug.Log("Logged in as: "+user.e_mail);
        }
        return temp;
    }

    public Dictionary<ushort, Player> playerList { get; private set; }

    private void Start()
    {
        playerList = new Dictionary<ushort, Player>();
        setConnection();
    }
    

    

}
