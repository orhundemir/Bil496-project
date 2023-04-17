using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class WallSerializer : MonoBehaviour{
    public GameObject[] gameObjectList;
    private List<Wall> wallList;
    void Start()
        {
            wallList = new List<Wall>();
        }
    public void Save(string fileName)
    {
        //Create binary formatter and a file stream object to write to a file in binary format
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(fileName, FileMode.Create);

        //to use when we load the scene back, write needed data
        foreach (var wall in gameObjectList)
        {
            formatter.Serialize(stream, wall.transform.Find("Hinge1").position.x);
            formatter.Serialize(stream, wall.transform.Find("Hinge1").position.y);
            formatter.Serialize(stream, wall.transform.Find("Hinge1").position.z);

            formatter.Serialize(stream, wall.transform.Find("Hinge2").position.x);
            formatter.Serialize(stream, wall.transform.Find("Hinge2").position.y);
            formatter.Serialize(stream, wall.transform.Find("Hinge2").position.z);

            //TO DO material
        }
        stream.Close();
    }

    public void Load(string fileName)
    {
        if (File.Exists(fileName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fileName, FileMode.Open);

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
                wall.wallMaterial = wallMaterial;

                wallList.Add(wall);

                //TO DO game object list
            }
            stream.Close();
        }
    }
}