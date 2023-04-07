using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Npgsql;

public class ConnectionManager{
    public static NpgsqlConnection  getConnection(){
        NpgsqlConnection conn = new NpgsqlConnection("Server = localhost; Port = 5432; User Id = postgres; Password = 123456; Database=IKEA;");//Connection string ypu can change it in your local but add this file to gitignore after that
        return conn;
    }
    public static void closeConnection(NpgsqlConnection conn){
        conn.Close();
    }
}