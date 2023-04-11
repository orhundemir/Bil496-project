using UnityEngine;
using HTC.UnityPlugin.Vive;

public class MenuController : MonoBehaviour
{

    [Header("Scene Menu Canvas")]
    [SerializeField] private GameObject sceneMenuCanvas;
    [SerializeField] private float sceneMenuCameraDistance = 1.2f;

    [Header("Scene Menu Canvas")]
    [SerializeField] private GameObject catalogMenuCanvas;
    [SerializeField] private float catalogMenuCameraDistance = 1.2f;

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

        sceneMenuCanvas.GetComponent<Canvas>().worldCamera = camera;
        catalogMenuCanvas.GetComponent<Canvas>().worldCamera = camera;
    }

    private void Update()
    {
        UpdateCanvasPositions();
    }

    private void UpdateCanvasPositions()
    {
        if (sceneMenuCanvas.active)
        {
            sceneMenuCanvas.transform.position = camera.transform.position + camera.transform.forward * sceneMenuCameraDistance;
            sceneMenuCanvas.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
        }
        if (catalogMenuCanvas.active)
        {
            catalogMenuCanvas.transform.position = camera.transform.position + camera.transform.forward * catalogMenuCameraDistance;
            catalogMenuCanvas.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
        }
    }

    private void MenuButtonListener()
    {
        bool sceneCanvasActive = sceneMenuCanvas.active;
        bool catalogCanvasActive = catalogMenuCanvas.active;
        sceneMenuCanvas.SetActive(!sceneMenuCanvas.active && !catalogCanvasActive);
        catalogMenuCanvas.SetActive(false);
    }

}
