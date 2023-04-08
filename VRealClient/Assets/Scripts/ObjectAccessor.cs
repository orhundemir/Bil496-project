using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAccessor : MonoBehaviour
{
    public GameObject[] gameObjectList;

    // A singleton object to access the functions and variables of this class
    // without any need to create object of this class every time
    // when new instance created gameObjectList will be empty by default !!!
    public static ObjectAccessor singletonInstance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (singletonInstance != null && singletonInstance != this)
        {
            Destroy(this);
        }
        else
        {
            singletonInstance = this;
        }

        // Before start no object should be placed on the scene
        foreach (var item in gameObjectList)
        {
            item.SetActive(false);
        }

    }

    public void SetActivetWithTheName(string requestedName)
    {
        //chechk the array and make requested object/furniture visible on the scene
        foreach (var item in gameObjectList)
        {
            if (item.name == requestedName)
            {
                item.transform.localPosition = new Vector3(0, 0, 0);
                item.SetActive(true);
            }
        }
    }

}
