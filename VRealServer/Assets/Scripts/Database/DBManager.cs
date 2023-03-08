using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;

public class DBManager : MonoBehaviour
{
    private static DBManager _singleton;

    private NpgsqlConnection conn;
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
    //Public NpgsqlConnextion object set within the parameters inside the ConnectionManager class
    public void setConnection(){
        conn = ConnectionManager.getConnection();
        openConnection();
    }
    //Returns NpgsqlConnection object
    public NpgsqlConnection getConnection(){
        return conn;
    }
    //Opens connection to the database
    public void openConnection(){
        conn.Open();
    }
    //Closes connection to the database too many idle connections can and will crash server
    public void closeConnection(){
        conn.Close();
    }
    public Dictionary<ushort, Player> playerList { get; private set; }

    private void Start()
    {
        playerList = new Dictionary<ushort, Player>();
        
    }
    

    

}
