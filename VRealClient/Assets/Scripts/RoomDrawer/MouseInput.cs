using UnityEngine;

public class MouseInput : MonoBehaviour
{

    public GameObject drawingArea;
    private BoxCollider drawingAreaCollider;

    private Vector3 clickPosition;

    public WallHandler wallHandler;
    private WallObject previewWall;

    void Start()
    {
        drawingAreaCollider = drawingArea.GetComponent<BoxCollider>();
    }

    void Update()
    {
        HandleMouseInputs();
    }

    // Detects the click and drag motion of the mouse and creates a wall object between it's start and end positions
    private void HandleMouseInputs()
    {
        // Instantiate the Wall Prefab at the mouse location when the left mouse button is first pressed
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.y;
            clickPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            GameObject wall = wallHandler.CreateWall(clickPosition);
            previewWall = wall.GetComponent<WallObject>();
            previewWall.ChangeMaterialToTransparent();
        }
        // Update the transparent preview wall object to follow the mouse position while the left mouse button is held down
        else if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.y;
            Vector3 endPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            wallHandler.UpdateWall(previewWall, clickPosition, endPosition);
        }

        // When the left mouse button is released, change the wall's material from transparent to opaque and stop updating that object
        if (Input.GetMouseButtonUp(0))
        {
            previewWall.ChangeMaterialToOpaque();
        }
    }

    // Checks if the given vector is contained inside the given collider
    private bool ColliderContainsVector(BoxCollider collider, Vector3 position)
    {
        return position.x >= collider.bounds.min.x &&
            position.x <= collider.bounds.max.x &&
            position.z >= collider.bounds.min.z &&
            position.z <= collider.bounds.max.z;
    }
}