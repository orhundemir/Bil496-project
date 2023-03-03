using UnityEngine;

public class MouseInputManager : MonoBehaviour
{

    public GameObject drawingArea;
    private BoxCollider drawingAreaCollider;

    public WallHandler wallHandler;
    public WallCreationManager wallCreationManager;
    public WallSelectionManager wallSelectionManager;

    void Start()
    {
        drawingAreaCollider = drawingArea.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        DelegateMouseInputs();
    }

    private void DelegateMouseInputs()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        if (Input.GetMouseButtonDown(0) && IsPositionValid(mousePosition))
        {
            if (EditorTools.selectedTool == EditorTools.TOOLS.SELECTOR)
                wallSelectionManager.HandleWallSelection();
            else if (EditorTools.selectedTool == EditorTools.TOOLS.WALL)
                wallCreationManager.StartWallCreation(wallHandler, mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            if (EditorTools.selectedTool == EditorTools.TOOLS.WALL)
            {
                mousePosition = SnapMouseToDrawingArea(mousePosition);
                wallCreationManager.UpdateWall(wallHandler, mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (EditorTools.selectedTool == EditorTools.TOOLS.WALL)
            {
                mousePosition = SnapMouseToDrawingArea(mousePosition);
                wallCreationManager.FinalizeWallCreation(wallHandler, mousePosition);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (EditorTools.selectedTool == EditorTools.TOOLS.SELECTOR)
            {
                wallSelectionManager.RemoveSelectedWalls();
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // Checks if the given position is contained inside the drawing area
    private bool IsPositionValid(Vector3 position)
    {
        return position.x >= drawingAreaCollider.bounds.min.x &&
            position.x <= drawingAreaCollider.bounds.max.x &&
            position.z >= drawingAreaCollider.bounds.min.z &&
            position.z <= drawingAreaCollider.bounds.max.z;
    }

    // Clamps the given position into the drawing area
    private Vector3 SnapMouseToDrawingArea(Vector3 position)
    {
        Vector3 colliderMin = drawingAreaCollider.bounds.min;
        Vector3 colliderMax = drawingAreaCollider.bounds.max;

        position.x = Mathf.Clamp(position.x, colliderMin.x, colliderMax.x);
        position.z = Mathf.Clamp(position.z, colliderMin.z, colliderMax.z);

        return position;
    }

}
