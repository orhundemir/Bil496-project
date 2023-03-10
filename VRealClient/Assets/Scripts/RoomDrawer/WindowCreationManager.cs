using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowCreationManager : MonoBehaviour
{

    [SerializeField] private GameObject windowPrefab;
    private WindowObject windowObject = null;

    private enum WindowCreationState
    {
        None,    // There is no existing window instance
        Invalid, // Current window is in an invalid position
        Valid    // Current window is in a valid position
    }
    private WindowCreationState currentState = WindowCreationState.None;

    public void HandleWindowCreation(Vector3 mousePosition)
    {
        GameObject wallAtMousePosition = GetObjectByTagOnRaycastHit(Input.mousePosition, "Wall");
        
        switch (currentState)
        {
            case WindowCreationState.None:
                // Create a new window instance if it does not exist
                if (windowObject == null)
                {
                    CreateWindow(mousePosition);
                    currentState = WindowCreationState.Invalid;
                }
                break;

            case WindowCreationState.Invalid:
                // If the mouse is not hovering over a wall, stay in the Invalid state and make the window follow the mouse
                if (wallAtMousePosition == null)
                {
                    windowObject.ChangeMaterialToInvalid();
                    MoveWindow(mousePosition);
                }
                // If the mouse is hovering over a wall, switch to the Valid state
                else
                {
                    currentState = WindowCreationState.Valid;
                }
                break;

            case WindowCreationState.Valid:
                // Return to the Invalid state if the mouse is not hovering over a wall
                if (wallAtMousePosition == null)
                {
                    currentState = WindowCreationState.Invalid;
                }
                // If the mouse is hovering over a wall, attach the window to it
                // The window will keep following the mouse as long as it is hovering over a wall
                else
                {
                    windowObject.ChangeMaterialToValid();
                    AttachWindowToWall(wallAtMousePosition, mousePosition);
                }
                break;
        }
    }

    // Create a new window instance at the given position and switch to the invalid state as default
    private void CreateWindow(Vector3 position)
    {
        GameObject window = Instantiate(windowPrefab, position, Quaternion.Euler(0, 90f, 0), transform);

        windowObject = window.GetComponent<WindowObject>();
        window.transform.localScale = new Vector3(.6f, 0.1f, windowObject.GetLength());
        windowObject.ChangeMaterialToInvalid();
    }

    // Move the window towards the given position, this is being used to make the window follow the mouse
    private void MoveWindow(Vector3 position)
    {
        windowObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
        windowObject.transform.position = new Vector3(position.x, 0f, position.z);

        windowObject.transform.SetParent(transform);
    }

    // Rotates the window to match the wall's rotation
    // Repositions the wall so that it is centered with the wall while still following the mouse
    // Limits the window's position to make sure it is fully contained within the wall, preventing overflow
    private void AttachWindowToWall(GameObject wall, Vector3 mousePosition)
    {
        WallObject wallObject = wall.transform.parent.parent.GetComponent<WallObject>();
        Vector3 wallStartPosition = wallObject.hinge1.transform.position;
        Vector3 wallEndPosition = wallObject.hinge2.transform.position;

        // Find the projection point and use it to match the window's center the the wall's
        Vector3 projection = CalculateProjectionPoint(wallStartPosition, wallEndPosition, mousePosition);

        windowObject.transform.position = wallStartPosition + projection + new Vector3(0, 0.05f, 0);
        windowObject.transform.rotation = wall.transform.rotation;
        // Place the window object inside the wall prefab in the hierarchy
        windowObject.transform.SetParent(wall.transform.parent.parent);
    }

    private Vector3 CalculateProjectionPoint(Vector3 wallStartPosition, Vector3 wallEndPosition, Vector3 mousePosition)
    {
        Vector3 wallVector = wallEndPosition - wallStartPosition;
        Vector3 mouseVector = mousePosition - wallStartPosition;
        Vector3 projection = Vector3.Project(mouseVector, wallVector);

        float projectionLength = projection.magnitude;
        float wallLength = wallVector.magnitude;
        float windowLength = windowObject.transform.localScale.z;

        if (projectionLength + windowLength / 2f > wallLength)
            projection = wallVector.normalized * (wallLength - windowLength / 2f);
        else if (projectionLength - windowLength / 2f < 0f)
            projection = wallVector.normalized * (windowLength / 2f);

        return projection;
    }

    // If the window is in a valid state, finalize it's creation process
    public void TryToPlaceWindow()
    {
        if (currentState == WindowCreationState.Valid)
        {
            windowObject.ChangeMaterialToWindow();
            windowObject = null;

            currentState = WindowCreationState.None;
        }
    }

    // Delete the window object and return back to the None state
    public void ResetWindow()
    {
        if (currentState != WindowCreationState.None)
        {
            Destroy(windowObject.gameObject);
            windowObject = null;
            currentState = WindowCreationState.None;
        }
    }

    private GameObject GetObjectByTagOnRaycastHit(Vector3 position, string tag)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag(tag))
            {
                return hit.transform.gameObject;
            }
        }

        return null;
    }
}
