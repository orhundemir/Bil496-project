using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateInTheScene : MonoBehaviour
{
    public void PutObjectToTheScene()
    {
        ObjectAccesser.singletonInstance.SetActivetWithTheName(this.name);
    }
}
