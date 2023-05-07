using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Riptide;
using System.Text;
using System;

public class WallSerializer : MonoBehaviour {

    public GameObject[] gameObjectList;
    public GameObject[] furniture;

    private List<Wall> wallList;
    private List<Furniture> furnitureList;

    public string wall;
    public string products;
    public string ceiling;
    public string floor;

    void Start()
    {
        wallList = new List<Wall>();
        furnitureList = new List<Furniture>();
    }

    public void Save()
    {
        // Turn all room entity objects into objects of the Wall class
        Transform wallsParent = GameObject.Find("Walls").transform;
        for (int i = 0; i < wallsParent.transform.childCount; i++)
        {
            GameObject obj = wallsParent.transform.GetChild(i).gameObject;
            Wall wall = new Wall();
            wall.position = obj.transform.position;
            wall.rotation = obj.transform.eulerAngles;
            wall.scale = obj.transform.localScale;

            if (obj.CompareTag("Ceiling") || obj.CompareTag("Floor"))
                wall.material = obj.transform.GetComponent<Renderer>().material.name;
            else
            {
                wall.material = obj.transform.GetChild(0).GetComponent<Renderer>().material.name;
                if (obj.CompareTag("Door"))
                    wall.color = new Color(1, 1, 1);
                else
                    wall.color = obj.transform.GetChild(0).GetComponent<Renderer>().material.color;
            }

            if (obj.CompareTag("Wall")) wall.type = 0;
            else if (obj.CompareTag("Window")) wall.type = 1;
            else if (obj.CompareTag("Door")) wall.type = 2;
            else if (obj.CompareTag("Ceiling")) wall.type = 3;
            else if (obj.CompareTag("Floor")) wall.type = 4;

            wallList.Add(wall);
        }

        StringBuilder stringBuilder = new StringBuilder(); 
        foreach (Wall wall in wallList)
        {
            stringBuilder.Append(wall.type + "-_-");
            stringBuilder.Append(wall.position.x.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.position.y.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.position.z.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.rotation.x.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.rotation.y.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.rotation.z.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.scale.x.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.scale.y.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.scale.z.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.material + "-_-");
            stringBuilder.Append(wall.color.r.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.color.g.ToString("0.#####") + "-_-");
            stringBuilder.Append(wall.color.b.ToString("0.#####") + "***");
        }

        GameObject furnitureParent = GameObject.Find("Furnitures");
        StringBuilder stringBuilder2 = new StringBuilder();
        for (int i = 0; i < furnitureParent.transform.childCount; i++)
        {
            GameObject item = furnitureParent.transform.GetChild(i).gameObject;
            stringBuilder2.Append(item.transform.localPosition.x.ToString("0.#####") + "-_-");
            stringBuilder2.Append(item.transform.localPosition.y.ToString("0.#####") + "-_-");
            stringBuilder2.Append(item.transform.localPosition.z.ToString("0.#####") + "-_-");
            stringBuilder2.Append(item.transform.eulerAngles.x.ToString("0.#####") + "-_-");
            stringBuilder2.Append(item.transform.eulerAngles.y.ToString("0.#####") + "-_-");
            stringBuilder2.Append(item.transform.eulerAngles.z.ToString("0.#####") + "-_-");
            stringBuilder2.Append(item.transform.localScale.x.ToString("0.#####") + "-_-");
            stringBuilder2.Append(item.transform.localScale.y.ToString("0.#####") + "-_-");
            stringBuilder2.Append(item.transform.localScale.z.ToString("0.#####") + "-_-");
            stringBuilder2.Append(item.name + "***");
        }

        wall = stringBuilder.ToString();
        products = stringBuilder2.ToString();

        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.roomTemplate);
        message.AddString(wall);
        message.AddString(products);
        NetworkManager.Singleton.Client.Send(message);
    }

    private void Load()
    {
        //TO DO:
        // wallar *** gore ayrilmasi gerekiyor. arr= string.split(***) -> [x,y,z...name] [x2,y2,z2 .... name2] ......
        // split edilen array -_- gore split edilecek [x],[y],[z]...[name] -> bu bilgilerle game object oluþturulacak.
        // olusturulan game obje walls veya product listesine eklenecek.
        //Player.list[NetworkManager.Singleton.Client.Id].Walls.Add();
        if (File.Exists("walls.bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream("walls.bin", FileMode.Open);

            //for every model, set position, scale and rotation with the data that we read from file
            for (int i = 0; i < gameObjectList.Length; i++)
            {
                int type = (int)formatter.Deserialize(stream);
                float x = (float)formatter.Deserialize(stream);
                float y = (float)formatter.Deserialize(stream);
                float z = (float)formatter.Deserialize(stream);
                float rotationX = (float)formatter.Deserialize(stream);
                float rotationY = (float)formatter.Deserialize(stream);
                float rotationZ = (float)formatter.Deserialize(stream);
                float scaleX = (float)formatter.Deserialize(stream);
                float scaleY = (float)formatter.Deserialize(stream);
                float scaleZ = (float)formatter.Deserialize(stream);
                string materialName = (string)formatter.Deserialize(stream);
                //Material wallMaterial = (Material)formatter.Deserialize(stream);

                Wall wall = new Wall();
                wall.type = type;
                wall.position = new Vector3(x, y, z);
                wall.rotation = new Vector3(rotationX, rotationY, rotationZ);
                wall.scale = new Vector3(scaleX, scaleY, scaleZ);
                wall.material = materialName;

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
                float scalex = (float)formatter.Deserialize(stream);
                float scaley = (float)formatter.Deserialize(stream);
                float scalez = (float)formatter.Deserialize(stream);
                string name = (string)formatter.Deserialize(stream);

                Furniture f = new Furniture();
                f.posX = posx;
                f.posY = posy;
                f.posZ = posz;
                f.rotX = rotx;
                f.rotY = roty;
                f.rotZ = rotz;
                f.scaleX = scalex;
                f.scaleY = scaley;
                f.scaleZ = scalez;
                f.name = name;
                f.isActive = true;
                furnitureList.Add(f);

                furniture[i].transform.localPosition = new Vector3(posx, posy, posz);
                furniture[i].transform.localRotation = Quaternion.Euler(rotx, roty, rotz);
                furniture[i].transform.localScale = new Vector3(scalex, scaley, scalez);

            }
            stream.Close();
        }
    }

    private string getString(string filePath){//Method for reading binary files
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        {
            byte[] buffer = new byte[4096];

            StringBuilder stringBuilder = new StringBuilder();
            int bytesRead;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                stringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                Debug.Log(stringBuilder.ToString());
            }
            string ret = stringBuilder.ToString();
            return ret;
        }
    }

}