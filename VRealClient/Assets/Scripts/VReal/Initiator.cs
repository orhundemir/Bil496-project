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

        //XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
       // XRGeneralSettings.Instance.Manager.StartSubsystems();

        Player temp = Player.list[NetworkManager.Singleton.Client.Id];
        VRModule.Initialize();
        
        //if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        //{
        //    Debug.LogWarning("Initializing XR Failed. Check Editor or Player log for details.");
        //}
        //else
        //{
        //    Debug.Log("Starting XR...");
        //    XRGeneralSettings.Instance.Manager.StartSubsystems();
        //}
        
        Player player = Instantiate(GameLogic.Singleton.VROrigin, temp.transform.position, temp.transform.rotation).GetComponent<Player>();
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
    
}
