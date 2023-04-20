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
    void Awake()
    {
        
        //VR gozluk kullanilacaksa asagidaki yorum satirlari acilmalidir.
        //XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        //XRGeneralSettings.Instance.Manager.StartSubsystems();
        VRModule.Initialize();

        //VR icin player olusturuluyor.
        Player temp = Player.list[NetworkManager.Singleton.Client.Id];
        Player player = Instantiate(GameLogic.Singleton.VROrigin, temp.transform.position, temp.transform.rotation).GetComponent<Player>();
        temp.CopyPlayer(player);
        //duvar,zemin,tavan ve isiklandirma objeleri olusturuluyor.
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
    
}
