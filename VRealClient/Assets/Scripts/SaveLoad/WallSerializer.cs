using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Riptide;
using System.Text;

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
            {
                wall.type = 1;
                wall.material = obj.transform.GetComponent<Renderer>().material.name;
            }
            else
            {
                wall.type = 0;
                wall.material = obj.transform.GetChild(0).GetComponent<Renderer>().material.name;
            }

            wallList.Add(wall);
        }

        //Create binary formatter and a file stream object to write to a file in binary format
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream("walls.bin", FileMode.Create);

        //to use when we load the scene back, write needed data
        foreach (Wall wall in wallList)
        {
            formatter.Serialize(stream, wall.type);

            formatter.Serialize(stream, wall.position.x);
            formatter.Serialize(stream, wall.position.y);
            formatter.Serialize(stream, wall.position.z);

            formatter.Serialize(stream, wall.rotation.x);
            formatter.Serialize(stream, wall.rotation.y);
            formatter.Serialize(stream, wall.rotation.z);

            formatter.Serialize(stream, wall.scale.x);
            formatter.Serialize(stream, wall.scale.y);
            formatter.Serialize(stream, wall.scale.z);

            formatter.Serialize(stream, wall.material);
        }
        stream.Close();

        GameObject furnitureParent = GameObject.Find("Furnitures");
        BinaryFormatter formatter2 = new BinaryFormatter();
        FileStream stream2 = new FileStream("furnitures.bin", FileMode.Create);
        // Save furnitures in the scene into the furnitures.bin file
        for (int i = 0; i < furnitureParent.transform.childCount; i++)
        {
            GameObject item = furnitureParent.transform.GetChild(i).gameObject;

            formatter2.Serialize(stream2, item.transform.localPosition.x);
            formatter2.Serialize(stream2, item.transform.localPosition.y);
            formatter2.Serialize(stream2, item.transform.localPosition.z);

            formatter2.Serialize(stream2, item.transform.eulerAngles.x);
            formatter2.Serialize(stream2, item.transform.eulerAngles.y);
            formatter2.Serialize(stream2, item.transform.eulerAngles.z);

            formatter2.Serialize(stream2, item.transform.localScale.x);
            formatter2.Serialize(stream2, item.transform.localScale.y);
            formatter2.Serialize(stream2, item.transform.localScale.z);

            formatter2.Serialize(stream2, item.name);
        }
        stream2.Close();
        
        wall = getString("walls.bin");
        products = getString("furnitures.bin");

        
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.roomTemplate);
        message.AddString(wall);
        message.AddString(products);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void createLoadFile(string w, string f){
        byte[] textDataw = Encoding.UTF8.GetBytes(w);
        byte[] textDataf = Encoding.UTF8.GetBytes(f);
        FileStream stream = new FileStream("walls.bin", FileMode.Create);
        FileStream stream2 = new FileStream("furnitures.bin", FileMode.Create);

        stream.Write(textDataw, 0, textDataw.Length);
        stream2.Write(textDataf, 0, textDataf.Length);
        
        stream.Close();
        stream2.Close();
        Load();
    }

    private void Load()
    {
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

                /* TODO
                 * VReal sahnesine zemin ve tavan haricindeki objeler parent-child olarak ikili olarak gonderiliyor
                 * O objeler load edilirken ayni hiyerarsi olusturulmali
                 * Asil 3 boyutlu sekli child'lar iceriyor
                 * Duvarlar icin child'larin z pozisyonu 0.5f, kalan butun objelerin childlarinin tum transformlari default
                 * 
                 * Tavan ve zemin icin child-parent iliskisine gerek yok, Wall class'ina bir de type ekledim.
                 * Type = 1 zemin ve tavan icin, Type = 0 kalan her sey icin
                 * Sadece Type = 0 olanlar icin bu hiyerarsi olusturulmali
                 */

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
            }
            string ret = stringBuilder.ToString();
            return ret;
        }
    }

}