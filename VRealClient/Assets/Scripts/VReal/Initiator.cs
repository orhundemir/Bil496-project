using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Initiator : MonoBehaviour
{

    [SerializeField] private GameObject RoomLight;

    private Player player;

    void Awake()
    {
        Player temp = Player.list[NetworkManager.Singleton.Client.Id];
        player = Instantiate(GameLogic.Singleton.VROrigin, temp.transform.position, temp.transform.rotation).GetComponent<Player>();
        temp.CopyPlayer(player);

        foreach (GameObject go in player.Walls)
        {
            player.SpawnWalls(go);
        }

        Instantiate(player.Ceiling);
        Instantiate(player.Floor);
        Vector3 lightPos = player.Ceiling.transform.position;
        lightPos.y = 2.5f;
        Instantiate(RoomLight, lightPos, Quaternion.identity);
    }

    public Vector3 GetRoomCenter()
    {
        return player.RoomCenter;
    }

}
