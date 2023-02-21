using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    private static DBManager _singleton;
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

    //FIX ME: Connection kurmayý saðlayan obje Global deðiþken olmalýdýr.
    //public DBConnection dbConn { get; set; }
    public Dictionary<ushort, Player> playerList { get; private set; }

    private void Start()
    {
        playerList = new Dictionary<ushort, Player>();
        
    }
    

    

}
