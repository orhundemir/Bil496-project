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

}