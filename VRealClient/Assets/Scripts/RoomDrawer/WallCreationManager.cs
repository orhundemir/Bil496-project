using UnityEngine;

public class WallCreationManager : MonoBehaviour
{

    public WallUIManager uiManager;

    private Vector3 clickPosition;
    private WallObject previewWall;
    private bool isDrawing = false;

    public GameObject wallPrefab;

    // Defines the scale/meter ratio
    private float lenghRatio = 2f;

    // Initialize the wall creation process at the given mouse position
    public void StartWallCreation(Vector3 mousePosition)
    {
        isDrawing = true;

        // If the mouse is clicked while on top of a hinge object, set the starting position of the wall to its center
        clickPosition = AdjustPositionForHinge(mousePosition);

        GameObject wall = CreateWall(clickPosition);
        previewWall = wall.GetComponent<WallObject>();
        previewWall.ChangeWallMaterialToTransparent();

        uiManager.SetCanvasPosition(clickPosition + new Vector3(0, 1f, 1f));
        uiManager.SetActive(true);
    }

    // Update the transparent preview wall object to follow the mouse position while the left mouse button is held down
    public void UpdateWall(Vector3 mousePosition)
    {
        if (isDrawing)
        {
            UpdateWallTransformValues(previewWall, clickPosition, mousePosition, true, out Vector3 wallCenter, out int wallAngle, out double wallLength);
            
            uiManager.SetLengthTextPosition(wallCenter);
            uiManager.SetAngleValue(wallAngle);
            uiManager.SetLengthValue(wallLength, 2);
        }
    }

    public void FinalizeWallCreation(Vector3 mousePosition)
    {
        if (isDrawing)
        {
            isDrawing = false;

            // If the mouse is released on a hinge, move the walls ending position to its center
            Vector3 releasePosition = AdjustPositionForHinge(mousePosition);
            // Update the wall one last time to apply the possible position change
            UpdateWallTransformValues(previewWall, clickPosition, releasePosition, false, out _, out _, out _);

            // Change its material from transparent to opaque and place hinges at both ends of it
            previewWall.ChangeWallMaterialToChosenTexture();
            previewWall.AdjustHingePositions(clickPosition, releasePosition);
            previewWall.ActivateHinges();

            uiManager.SetActive(false);
        }
    }

    // Create a Wall at the given position and add it to the game
    public GameObject CreateWall(Vector3 position)
    {
        return Instantiate(wallPrefab, position, Quaternion.identity, transform);
    }

    // Update the given WallObject's transform values so that it is drawn between the given start and end vectors
    public void UpdateWallTransformValues(WallObject wallObject, Vector3 start, Vector3 end, bool limitAngles, out Vector3 centerPosition, out int wallAngle, out double wallLength)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        direction.Normalize();

        // Round the angle of rotation to the nearest integer and rotate the wall object accordingly
        int angle = 0;
        if (distance > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            angle = Mathf.RoundToInt(rotation.eulerAngles.y);
            if (limitAngles)
            {
                rotation.eulerAngles = new Vector3(0, angle, 0);
                direction = Quaternion.Euler(0, -90, 0) * rotation * Vector3.right;
            }
            wallObject.transform.rotation = rotation;
        }

        Vector3 scale = new Vector3(wallObject.GetWidth(), wallObject.GetHeight(), distance);
        wallObject.transform.GetChild(0).localScale = scale;

        // Calculate the output values, they will be passed into WallUIManager later
        wallAngle = angle - 90;
        centerPosition = start + (direction * distance) / 2f + new Vector3(0, 0.5f, 0);
        wallLength = wallObject.transform.GetChild(0).localScale.z / lenghRatio;
    }

    // If the given position is contained inside a hinge, this method moves the position to the hinge's center
    // Thus, ensuring that walls are connected properly during the drawing process
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
