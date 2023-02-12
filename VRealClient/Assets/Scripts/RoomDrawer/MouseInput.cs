using UnityEngine;

public class MouseInput : MonoBehaviour
{

    public GameObject drawingArea;
    private BoxCollider drawingAreaCollider;

    private Vector3 clickPosition;

    public WallHandler wallHandler;
    private bool isDrawing = false;
    private WallObject previewWall;

    void Start()
    {
        drawingAreaCollider = drawingArea.GetComponent<BoxCollider>();
    }

    void Update()
    {
        HandleDragging();
    }

    // Detects the dragging motion of the mouse and uses its start and end coordinates to create a new wall
    private void HandleDragging()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.y;

            // Display a preview of the Wall that will be created as the user drags the mouse, and update the preview in real-time until the mouse is released
            if (!isDrawing)
            {
                isDrawing = true;

                clickPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                GameObject wall = wallHandler.CreateWall(clickPosition);

                previewWall = wall.GetComponent<WallObject>();
                previewWall.ChangeMaterialToTransparent();
            }
            else
            {
                Vector3 endPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                previewWall = wallHandler.UpdateWall(previewWall, clickPosition, endPosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
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