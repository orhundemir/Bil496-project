using UnityEngine;
using UnityEngine.UI;

public class MouseInput : MonoBehaviour
{

    public GameObject drawingArea;
    private BoxCollider drawingAreaCollider;

    private Vector3 clickPosition;

    public WallHandler wallHandler;
    private WallObject previewWall;
    private bool isDrawing = false;

    public GameObject wallInfoCanvas;
    public Text wallInfoText, angleText;

    void Start()
    {
        drawingAreaCollider = drawingArea.GetComponent<BoxCollider>();
    }

    void Update()
    {
        HandleMouseInputs();
    }

    // Detects the click and drag motion of the mouse and creates a wall object between its start and end positions
    private void HandleMouseInputs()
    {
        Vector3 mousePosition = GetMousePosition();

        // Initialize the wall creation process at the current mouse position
        if (Input.GetMouseButtonDown(0) && ColliderContainsVector(drawingAreaCollider, mousePosition))
        {
            StartWallCreation(mousePosition);

            wallInfoCanvas.transform.position = mousePosition + new Vector3(0, 1f, 15f);
            wallInfoCanvas.SetActive(true);
        }
        // Update the transparent preview wall object to follow the mouse position while the left mouse button is held down
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            mousePosition = SnapMouseToDrawingArea(drawingAreaCollider, mousePosition);
            wallHandler.UpdateWall(previewWall, clickPosition, mousePosition, wallInfoText, angleText);
        }

        // When the left mouse button is released, stop updating the wall object
        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            mousePosition = SnapMouseToDrawingArea(drawingAreaCollider, mousePosition);
            FinalizeWallCreation(mousePosition);
            wallInfoCanvas.SetActive(false);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void StartWallCreation(Vector3 mousePosition)
    {
        isDrawing = true;

        // If the mouse is clicked while on top of a hinge object, set the starting position of the wall to its center
        clickPosition = AdjustPositionForHinge(mousePosition);

        GameObject wall = wallHandler.CreateWall(clickPosition);
        previewWall = wall.GetComponent<WallObject>();
        previewWall.ChangeWallMaterialToTransparent();
    }

    private void FinalizeWallCreation(Vector3 mousePosition)
    {
        isDrawing = false;

        // If the mouse is released while on top of a hinge object, set the ending position of the wall to its center
        Vector3 releasePosition = AdjustPositionForHinge(mousePosition);
        wallHandler.UpdateWall(previewWall, clickPosition, releasePosition, wallInfoText, angleText);

        // Change its material from transparent to opaque and place hinges at both ends of it
        previewWall.ChangeWallMaterialToOpaque();
        wallHandler.ActivateHinges(previewWall);
    }

    // Check if there is a hinge object at the given position and move the vector to its center
    private Vector3 AdjustPositionForHinge(Vector3 position)
    {
        GameObject hingeAtMousePosition = GetObjectByTagOnRaycastHit(Input.mousePosition, "Hinge");
        if (hingeAtMousePosition != null)
        {
            position.x = hingeAtMousePosition.transform.position.x;
            position.z = hingeAtMousePosition.transform.position.z;
        }

        return position;
    }

    // Raycast from the main camera to the mouse position and return a hinge object if the ray hits one
    private GameObject GetObjectByTagOnRaycastHit(Vector3 position, string tag)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
            if (hit.collider.gameObject.CompareTag(tag))
                return hit.transform.gameObject;

        return null;
    }

    // Checks if the given vector is contained inside the given collider
    private bool ColliderContainsVector(BoxCollider collider, Vector3 position)
    {
        return position.x >= collider.bounds.min.x &&
            position.x <= collider.bounds.max.x &&
            position.z >= collider.bounds.min.z &&
            position.z <= collider.bounds.max.z;
    }

    // Ensures that the mouse position is within the bounds of the drawing area collider
    private Vector3 SnapMouseToDrawingArea(BoxCollider collider, Vector3 position)
    {
        Vector3 colliderMin = collider.bounds.min;
        Vector3 colliderMax = collider.bounds.max;

        position.x = Mathf.Clamp(position.x, colliderMin.x, colliderMax.x);
        position.z = Mathf.Clamp(position.z, colliderMin.z, colliderMax.z);

        return position;
    }

}
