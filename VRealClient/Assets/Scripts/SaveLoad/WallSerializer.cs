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
                wall.material = obj.transform.GetComponent<Renderer>().material.name;
            else
                wall.material = obj.transform.GetChild(0).GetComponent<Renderer>().material.name;

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
            stringBuilder.Append(wall.position.x + "-_-");
            stringBuilder.Append(wall.position.y + "-_-");
            stringBuilder.Append(wall.position.z + "-_-");
            stringBuilder.Append(wall.rotation.x + "-_-");
            stringBuilder.Append(wall.rotation.y + "-_-");
            stringBuilder.Append(wall.rotation.z + "-_-");
            stringBuilder.Append(wall.scale.x + "-_-");
            stringBuilder.Append(wall.scale.y + "-_-");
            stringBuilder.Append(wall.scale.z + "-_-");
            stringBuilder.Append(wall.material + "***");
        }
        

        GameObject furnitureParent = GameObject.Find("Furnitures");
        StringBuilder stringBuilder2 = new StringBuilder();
        // Save furnitures in the scene into the furnitures.bin file
        for (int i = 0; i < furnitureParent.transform.childCount; i++)
        {
            GameObject item = furnitureParent.transform.GetChild(i).gameObject;
            stringBuilder2.Append(item.transform.localPosition.x + "-_-");
            stringBuilder2.Append(item.transform.localPosition.y + "-_-");
            stringBuilder2.Append(item.transform.localPosition.z + "-_-");
            stringBuilder2.Append(item.transform.eulerAngles.x + "-_-");
            stringBuilder2.Append(item.transform.eulerAngles.y + "-_-");
            stringBuilder2.Append(item.transform.eulerAngles.z + "-_-");
            stringBuilder2.Append(item.transform.localScale.x + "-_-");
            stringBuilder2.Append(item.transform.localScale.y + "-_-");
            stringBuilder2.Append(item.transform.localScale.z + "-_-");
            stringBuilder2.Append(item.name + "***");
        }


        wall = stringBuilder.ToString();
        products = stringBuilder2.ToString();
        Debug.Log(wall + "\n\n\n" + products);
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
                Debug.Log(stringBuilder.ToString());
            }
            string ret = stringBuilder.ToString();
            return ret;
        }
    }

}