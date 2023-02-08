using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData
{
    public float posX { get; set; }
    public float posY { get; set; }
    public float posZ { get; set; }
    public float rotX { get; set; }
    public float rotY { get; set; }
    public float rotZ { get; set; }
    public bool isActive { get; set; }
    public string name { get; set; }
}

public class SaveLoadSampleScript : MonoBehaviour
{
    //list of added models(game objects)
    public GameObject[] gameObjectList;

    //list of the model data in the same order with gameObjectList
    //likely to be used in the future
    private List<GameData> gameDataList;

    // Start is called before the first frame update
    void Start()
    {
        gameDataList = new List<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
            Save("deneme.bin");

        else if (Input.GetKey(KeyCode.L))
            Load("deneme.bin");

    }


    public void Save(string fileName)
    {
        //Create binary formatter and a file stream object to write to a file in binary format
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(fileName, FileMode.Create);

        //to use when we load the scene back, write needed data
        foreach (var item in gameObjectList)
        {
            //position data
            formatter.Serialize(stream, item.transform.localPosition.x);
            formatter.Serialize(stream, item.transform.localPosition.y);
            formatter.Serialize(stream, item.transform.localPosition.z);

            //rotation data
            formatter.Serialize(stream, item.transform.eulerAngles.x);
            formatter.Serialize(stream, item.transform.eulerAngles.y);
            formatter.Serialize(stream, item.transform.eulerAngles.z);

            //other potential needed data
            formatter.Serialize(stream, item.activeSelf);
            formatter.Serialize(stream, item.name);
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
                float posx = (float)formatter.Deserialize(stream);
                float posy = (float)formatter.Deserialize(stream);
                float posz = (float)formatter.Deserialize(stream);
                float rotx = (float)formatter.Deserialize(stream);
                float roty = (float)formatter.Deserialize(stream);
                float rotz = (float)formatter.Deserialize(stream);
                bool isActive = (bool)formatter.Deserialize(stream);
                string name = (string)formatter.Deserialize(stream);

                GameData tempGameDataObject = new GameData();
                tempGameDataObject.posX = posx;
                tempGameDataObject.posY = posy;
                tempGameDataObject.posZ = posz;
                tempGameDataObject.rotX = rotx;
                tempGameDataObject.rotY = roty;
                tempGameDataObject.rotZ = rotz;
                tempGameDataObject.isActive = isActive;
                tempGameDataObject.name = name;
                gameDataList.Add(tempGameDataObject);

                gameObjectList[i].transform.localPosition = new Vector3(posx, posy, posz);

                //to not move continuously when consecutive load applied
                gameObjectList[i].transform.localRotation = Quaternion.Euler(0, 0, 0);
                gameObjectList[i].transform.Rotate(rotx, roty, rotz);

            }
            stream.Close();
        }
    }
}
