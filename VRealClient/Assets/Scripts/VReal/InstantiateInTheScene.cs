using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class InstantiateInTheScene : MonoBehaviour
{

    private GameObject root;
    private Vector3 roomCenter;

    public void Start()
    {
        root = GameObject.Find("Furnitures");
        roomCenter = GameObject.Find("Initiator").GetComponent<Initiator>().GetRoomCenter();
    }

    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {   
        var loadedObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/VReal/Furnitures/" + filename + ".3ds");
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
    }

    public void PutObjectToTheScene()
    {
        var loadedPrefabResource = LoadPrefabFromFile(this.name);
        Instantiate(loadedPrefabResource, roomCenter, Quaternion.identity, root.transform);
    }
}
