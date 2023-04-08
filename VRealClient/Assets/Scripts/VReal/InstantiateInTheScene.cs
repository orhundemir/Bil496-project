using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateInTheScene : MonoBehaviour
{
    public void PutObjectToTheScene()
    {
        ObjectAccessor.singletonInstance.SetActivetWithTheName(this.name);
    }
}
