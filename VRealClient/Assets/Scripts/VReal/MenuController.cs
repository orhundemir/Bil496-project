using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject sceneMenuCanvas;
    private Camera camera;

    void Start()
    {
        camera = FindObjectOfType<Camera>();

        if (camera == null)
            Debug.LogError("Could not find a camera in the scene");
    }


    private void Update()
    {
        // Listens to the Menu button on the right hand controller
        if (Input.GetKeyDown(KeyCode.Space) || ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Menu)) {
            if (sceneMenuCanvas.active)
                sceneMenuCanvas.SetActive(false);
            else
            {
                sceneMenuCanvas.transform.position = camera.transform.position + camera.transform.forward;
                sceneMenuCanvas.transform.rotation = Quaternion.LookRotation(camera.transform.forward);

                sceneMenuCanvas.SetActive(true);
            }
        }
    }

}
