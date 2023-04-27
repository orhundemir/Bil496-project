using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class WallSerializer : MonoBehaviour{
    public GameObject[] gameObjectList;
    public GameObject[] furniture;
    private List<Wall> wallList;
    private List<Furniture> furnitureList;
    Player player;

    void Start()
        {
            wallList = new List<Wall>();
        }
    public void Save()
    {
        GameObject walls = GameObject.FindGameObjectWithTag("WallObject");
        for (int i = 0; i < walls.transform.childCount; i++)
        {
            GameObject w = walls.transform.GetChild(i).gameObject;
            Wall wall = new Wall();
            Vector3 pos = w.transform.position;
            Vector3 scale = w.transform.localScale;
            wall.x1 = pos.x + scale.x;
            wall.x2 = pos.x - scale.x;
            wall.y1 = pos.y;
            wall.y2 = pos.y;
            wall.z1 = pos.z + scale.z;
            wall.z2 = pos.z - scale.z;
            wall.wallMaterial = w.transform.GetChild(0).GetComponent<Renderer>().material.name;
            wallList.Add(wall);
        }

        //Create binary formatter and a file stream object to write to a file in binary format
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream("walls.bin", FileMode.Create);

        //to use when we load the scene back, write needed data
        foreach (var wall in wallList)
        {
            formatter.Serialize(stream, wall.x1);
            formatter.Serialize(stream, wall.y1);
            formatter.Serialize(stream, wall.z1);

            formatter.Serialize(stream, wall.x2);
            formatter.Serialize(stream, wall.y2);
            formatter.Serialize(stream, wall.z2);
            
            //formatter.Serialize(stream, wall.wallMaterial);

        }
        stream.Close();
        
        GameObject furnitures = GameObject.FindGameObjectWithTag("FurnitureObject");
        BinaryFormatter formatter2 = new BinaryFormatter();
        FileStream stream2 = new FileStream("furnitures.bin", FileMode.Create);

        foreach (var item in furniture)
        {
            if(item.activeSelf){
                formatter2.Serialize(stream, item.transform.localPosition.x);
                formatter2.Serialize(stream, item.transform.localPosition.y);
                formatter2.Serialize(stream, item.transform.localPosition.z);

                formatter2.Serialize(stream, item.transform.eulerAngles.x);
                formatter2.Serialize(stream, item.transform.eulerAngles.y);
                formatter2.Serialize(stream, item.transform.eulerAngles.z);

                formatter2.Serialize(stream, item.name);
            }
        }
        stream2.Close();
    }

    public void Load()
    {
        if (File.Exists("walls.bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream("walls.bin", FileMode.Open);

            //for every model, set position and rotation with the data that we read from file
            for (int i = 0; i < gameObjectList.Length; i++)
            {
                float x1 = (float)formatter.Deserialize(stream);
                float y1 = (float)formatter.Deserialize(stream);
                float z1 = (float)formatter.Deserialize(stream);

                float x2 = (float)formatter.Deserialize(stream);
                float y2 = (float)formatter.Deserialize(stream);
                float z2 = (float)formatter.Deserialize(stream);

                Material wallMaterial = (Material)formatter.Deserialize(stream);

                Wall wall = new Wall();
                wall.x1 = x1;
                wall.y1 = y1;
                wall.z1 = z1;
                wall.x2 = x2;
                wall.y2 = y2;
                wall.z2 = z2;
                wall.wallMaterial = wallMaterial.name;

                wallList.Add(wall);

            }
            stream.Close();
        }
        if (File.Exists("furnitures.bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream("furnitures.bin", FileMode.Open);

            for (int i = 0; i < gameObjectList.Length; i++)
            {
                float posx = (float)formatter.Deserialize(stream);
                float posy = (float)formatter.Deserialize(stream);
                float posz = (float)formatter.Deserialize(stream);
                float rotx = (float)formatter.Deserialize(stream);
                float roty = (float)formatter.Deserialize(stream);
                float rotz = (float)formatter.Deserialize(stream);
                string name = (string)formatter.Deserialize(stream);

                Furniture f = new Furniture();
                f.posX = posx;
                f.posY = posy;
                f.posZ = posz;
                f.rotX = rotx;
                f.rotY = roty;
                f.rotZ = rotz;
                f.name = name;
                f.isActive = true;
                furnitureList.Add(f);

                furniture[i].transform.localPosition = new Vector3(posx, posy, posz);
                furniture[i].transform.localRotation = Quaternion.Euler(rotx, roty, rotz);

            }
            stream.Close();
        }
    }
}