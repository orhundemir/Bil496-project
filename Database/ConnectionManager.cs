using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Npgsql;

public class Connection{
    NpgsqlConnection  getConnection(){
        NpgsqlConnection conn = new NpgsqlConnection("Server = localhost; Port = 5432; User Id = postgres; Password = admin; Database=IKEA;");//Connection string ypu can change it in your local but add this file to gitignore after that
        return conn;
    }
    void closeConnection(NpsqlConnection conn){
        conn.Close();
    }
    static void main(){
        Console.WriteLine("Hello");
    }
}