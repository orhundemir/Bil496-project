using UnityEngine;

public class MouseInput : MonoBehaviour {

    public GameObject drawingArea;
    private BoxCollider drawingAreaCollider;

    private Vector3 clickPosition;

    public WallHandler wallHandler;

    void Start() {
        drawingAreaCollider = drawingArea.GetComponent<BoxCollider>();
    }

    void Update() {
        HandleDragging();
    }

    // Detects the dragging motion of the mouse and uses its start and end coordinates to create a new wall
    private void HandleDragging() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.y;
            // Convert the 2D mouse coordinates into 3D world coordinates
            clickPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        else if (Input.GetMouseButtonUp(0)) {
            Vector3 mousePosition = Input.mousePosition;
            // Ensure that the wall is created at the correct height, relative to the camera
            mousePosition.z = Camera.main.transform.position.y;
            Vector3 releasePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (ColliderContainsVector(drawingAreaCollider, clickPosition) && ColliderContainsVector(drawingAreaCollider, releasePosition))
                wallHandler.AddWall(clickPosition, releasePosition);
        }
    }

    // Checks if the given vector is contained inside the given collider
    private bool ColliderContainsVector(BoxCollider collider, Vector3 position) {
        return position.x >= collider.bounds.min.x &&
            position.x <= collider.bounds.max.x &&
            position.z >= collider.bounds.min.z &&
            position.z <= collider.bounds.max.z;
    }
}