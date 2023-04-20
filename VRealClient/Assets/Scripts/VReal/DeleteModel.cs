using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteModel : MonoBehaviour
{
    
    public void SetAsGrabbed()
    {
        GameObject.Find("Furnitures").GetComponent<ModelDeletionManager>().SetGrabbedObject(gameObject);
    }

    public void SetAsNotGrabbed()
    {
        GameObject.Find("Furnitures").GetComponent<ModelDeletionManager>().ResetGrabbedObject();
    }

}
