using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HTC.UnityPlugin.VRModuleManagement;
using UnityEngine.XR.Management;
using UnityEngine.XR;

public class Initiator : MonoBehaviour
{

    [SerializeField] private GameObject RoomLight;

    private Player player;

    void Awake()
    {
        
        //VR gozluk kullanilacaksa asagidaki yorum satirlari acilmalidir.
        //XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        //XRGeneralSettings.Instance.Manager.StartSubsystems();
        VRModule.Initialize();

        //VR icin player olusturuluyor.
        Player temp = Player.list[NetworkManager.Singleton.Client.Id];
        player = Instantiate(GameLogic.Singleton.VROrigin, temp.transform.position, temp.transform.rotation).GetComponent<Player>();
        temp.CopyPlayer(player);
        //duvar,zemin,tavan ve isiklandirma objeleri olusturuluyor.
        GameObject wallsParent = new GameObject("Walls");
        foreach (GameObject go in player.Walls)
        {
            player.SpawnWalls(go, wallsParent.transform);
        }
        GameObject ceiling = Instantiate(player.Ceiling);
        ceiling.transform.parent = wallsParent.transform;
        GameObject floor = Instantiate(player.Floor);
        floor.transform.parent = wallsParent.transform;

        Vector3 lightPos = player.Ceiling.transform.position;
        lightPos.y = 2.5f;
        Instantiate(RoomLight, lightPos, Quaternion.identity);
    }

    public Vector3 GetRoomCenter()
    {
        return player.RoomCenter;
    }

}
