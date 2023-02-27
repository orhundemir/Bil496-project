using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Initiator : MonoBehaviour
{
    void Awake()
    {
        Player temp = Player.list[NetworkManager.Singleton.Client.Id];
        Player player = Instantiate(GameLogic.Singleton.VROrigin, temp.transform.position, temp.transform.rotation).GetComponent<Player>();
        temp.CopyPlayer(player);
        
        foreach (GameObject go in player.Walls)
        {
            player.SpawnWalls(go);
        }

    }
}
