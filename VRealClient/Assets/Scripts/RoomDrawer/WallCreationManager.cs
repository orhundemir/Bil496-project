using UnityEngine;

public class WallCreationManager : MonoBehaviour
{

    public WallUIManager uiManager;

    private Vector3 clickPosition;
    private WallObject previewWall;
    private bool isDrawing = false;

    // Initialize the wall creation process at the given mouse position
    public void StartWallCreation(WallHandler wallHandler, Vector3 mousePosition)
    {
        isDrawing = true;

        // If the mouse is clicked while on top of a hinge object, set the starting position of the wall to its center
        clickPosition = AdjustPositionForHinge(mousePosition);

        GameObject wall = wallHandler.CreateWall(clickPosition);
        previewWall = wall.GetComponent<WallObject>();
        previewWall.ChangeWallMaterialToTransparent();

        uiManager.SetCanvasPosition(clickPosition + new Vector3(0, 1f, 15f));
        uiManager.SetActive(true);
    }

    // Update the transparent preview wall object to follow the mouse position while the left mouse button is held down
    public void UpdateWall(WallHandler wallHandler, Vector3 mousePosition)
    {
        if (isDrawing)
        {
            Vector3 wallCenter;
            int wallAngle;
            double wallLength;
            wallHandler.UpdateWall(previewWall, clickPosition, mousePosition, true, out wallCenter, out wallAngle, out wallLength);
            
            uiManager.SetLengthTextPosition(wallCenter);
            uiManager.SetAngleValue(wallAngle);
            uiManager.SetLengthValue(wallLength, 2);
        }
    }

    public void FinalizeWallCreation(WallHandler wallHandler, Vector3 mousePosition)
    {
        isDrawing = false;

        // If the walls ending position is contained inside a hinge, move the position to its center
        Vector3 releasePosition = AdjustPositionForHinge(mousePosition);
        wallHandler.UpdateWall(previewWall, clickPosition, releasePosition, false);

        // Change its material from transparent to opaque and place hinges at both ends of it
        previewWall.ChangeWallMaterialToOpaque();
        previewWall.AdjustHingePositions(clickPosition, releasePosition);
        previewWall.ActivateHinges();

        uiManager.SetActive(false);
    }

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

    private GameObject GetObjectByTagOnRaycastHit(Vector3 position, string tag)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
            if (hit.collider.gameObject.CompareTag(tag))
                return hit.transform.gameObject;

        return null;
    }

}
