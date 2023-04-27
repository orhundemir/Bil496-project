using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class ModelDeletionManager : MonoBehaviour
{

    private GameObject grabbedObject;

    private void Awake()
    {
        ViveInput.AddListenerEx(HandRole.RightHand, ControllerButton.Grip, ButtonEventType.Down, DeleteGrabbedObject);
    }

    private void OnDestroy()
    {
        ViveInput.RemoveListenerEx(HandRole.RightHand, ControllerButton.Grip, ButtonEventType.Down, DeleteGrabbedObject);
    }

    private void DeleteGrabbedObject()
    {
        if (grabbedObject != null)
        {
            //grabbedObject.GetComponent<BasicGrabbable>().ForceRelease();
            //grabbedObject.SetActive(false);
            Destroy(grabbedObject);
            grabbedObject = null;
        }
    }

    public void SetGrabbedObject(GameObject grabbedObject)
    {
        this.grabbedObject = grabbedObject;
    }

    public void ResetGrabbedObject()
    {
        grabbedObject = null;
    }

}
