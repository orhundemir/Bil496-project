using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiator : MonoBehaviour
{
    [SerializeField] public Player player;
    // Start is called before the first frame update
    void Awake()
    {
        player = Player.list[NetworkManager.Singleton.Client.Id];
        foreach(GameObject go in player.Walls)
        {
            player.SpawnWalls(go);
        }
    }

 
}
