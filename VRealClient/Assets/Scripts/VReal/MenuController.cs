using UnityEngine;
using HTC.UnityPlugin.Vive;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject sceneMenuCanvas;
    [SerializeField] public float distanceFromCamera = 1f;

    private new Camera camera;

    // Assign listeners to controller buttons
    private void Awake()
    {
        ViveInput.AddListenerEx(HandRole.RightHand, ControllerButton.Menu, ButtonEventType.Down, MenuButtonListener);
    }

    // Unassign the controller button listeners
    private void OnDestroy()
    {
        ViveInput.RemoveListenerEx(HandRole.RightHand, ControllerButton.Trigger, ButtonEventType.Down, MenuButtonListener);
    }

    private void Start()
    {
        camera = FindObjectOfType<Camera>();

        if (camera == null)
            Debug.LogError("Could not find a camera in the scene");
    }


    private void Update()
    {
        if (sceneMenuCanvas.active) {
            sceneMenuCanvas.transform.position = camera.transform.position + camera.transform.forward * distanceFromCamera;
            sceneMenuCanvas.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
        }
    }

    private void MenuButtonListener()
    {
        sceneMenuCanvas.SetActive(!sceneMenuCanvas.active);
    }

}
