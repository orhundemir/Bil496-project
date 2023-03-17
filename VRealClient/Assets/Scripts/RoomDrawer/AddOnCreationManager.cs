using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnCreationManager : MonoBehaviour
{

    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private GameObject doorPrefab;

    private AddOnObject currentAddOnObject;

    public enum AddOnType
    {
        Window,
        Door
    }

    private enum AddOnCreationState
    {
        None,    // There is no existing add-on instance
        Invalid, // Current add-on is in an invalid position
        Valid    // Current add-on is in a valid position
    }
    private AddOnCreationState currentState = AddOnCreationState.None;

    public void HandleAddOnCreation(Vector3 mousePosition, AddOnType addOnType)
    {
        GameObject wallAtMousePosition = GetObjectByTagOnRaycastHit(Input.mousePosition, "Wall");

        switch (currentState)
        {
            case AddOnCreationState.None:
                // Create a new add-on instance of the selected type if it does not exist and switch to the Invalid state by default
                CreateAddOn(mousePosition, addOnType);
                currentState = AddOnCreationState.Invalid;
                break;

            case AddOnCreationState.Invalid:
                // If the mouse is not hovering over a wall, stay in the Invalid state and make the add-on follow the mouse
                if (wallAtMousePosition == null)
                {
                    currentAddOnObject.ChangeMaterialToInvalid();
                    MoveAddOn(mousePosition);
                }
                // If the mouse is hovering over a wall, switch to the Valid state
                else
                {
                    currentState = AddOnCreationState.Valid;
                }
                break;

            case AddOnCreationState.Valid:
                // If the mouse is not hovering over a wall, return to the Invalid state
                if (wallAtMousePosition == null)
                {
                    currentState = AddOnCreationState.Invalid;
                }
                // If the mouse is hovering over a wall, attach the add-on to it
                // The add-on will keep following the mouse as long as it is hovering over a wall
                else
                {
                    currentAddOnObject.ChangeMaterialToValid();
                    AttachAddOnToWall(wallAtMousePosition, mousePosition);
                }
                break;
        }
    }

    // Create a new add-on instance at the given position and switch to the Invalid state as default
    private void CreateAddOn(Vector3 position, AddOnType chosenAddOn)
    {
        GameObject addOn = null;

        if (chosenAddOn == AddOnType.Door)
            addOn = Instantiate(doorPrefab, position, Quaternion.Euler(0, 90f, 0), transform);
        else if (chosenAddOn == AddOnType.Window)
            addOn = Instantiate(windowPrefab, position, Quaternion.Euler(0, 90f, 0), transform);

        currentAddOnObject = addOn.GetComponent<AddOnObject>();
        addOn.transform.localScale = new Vector3(.6f, 0.1f, currentAddOnObject.GetLength());
        currentAddOnObject.ChangeMaterialToInvalid();
    }

    // Move the add-on towards the given position
    // This is being used to make the add-on follow the mouse during the Invalid state
    private void MoveAddOn(Vector3 position)
    {
        currentAddOnObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
        currentAddOnObject.transform.position = new Vector3(position.x, 0f, position.z);

        currentAddOnObject.transform.SetParent(transform);
    }

    // Rotates the add-on to match the wall's rotation
    // Repositions the add-on so that it is centered with the wall while still following the mouse
    // Limits the add-on's position to make sure it is fully contained within the wall, preventing overflow
    private void AttachAddOnToWall(GameObject wall, Vector3 mousePosition)
    {
        WallObject wallObject = wall.transform.parent.parent.GetComponent<WallObject>();
        Vector3 wallStartPosition = wallObject.hinge1.transform.position;
        Vector3 wallEndPosition = wallObject.hinge2.transform.position;

        // Find the projection point and use it to match the add-on's center the the wall's
        Vector3 projection = CalculateProjectionPoint(wallStartPosition, wallEndPosition, mousePosition);

        currentAddOnObject.transform.position = wallStartPosition + projection + new Vector3(0, 0.05f, 0);
        currentAddOnObject.transform.rotation = wall.transform.rotation;
        // Place the add-on object inside the wall prefab in the hierarchy
        currentAddOnObject.transform.SetParent(wall.transform.parent.parent);
    }

    private Vector3 CalculateProjectionPoint(Vector3 wallStartPosition, Vector3 wallEndPosition, Vector3 mousePosition)
    {
        Vector3 wallVector = wallEndPosition - wallStartPosition;
        Vector3 mouseVector = mousePosition - wallStartPosition;
        Vector3 projection = Vector3.Project(mouseVector, wallVector);

        float projectionLength = projection.magnitude;
        float wallLength = wallVector.magnitude;
        float windowLength = currentAddOnObject.transform.localScale.z;

        // Clamp the projection position to be inside the wall
        // Takes the add-on's length into account to prevent it from overflowing out of the wall
        if (projectionLength + windowLength / 2f > wallLength)
            projection = wallVector.normalized * (wallLength - windowLength / 2f);
        else if (projectionLength - windowLength / 2f < 0f)
            projection = wallVector.normalized * (windowLength / 2f);

        return projection;
    }

    // If the add-on is in the Valid state, place it on the wall that is currently being hovered and stop updating said add-on
    public void TryToPlaceAddOn()
    {
        if (currentState == AddOnCreationState.Valid)
        {
            currentAddOnObject.ChangeMaterialToFinal();
            currentAddOnObject = null;

            currentState = AddOnCreationState.None;
        }
    }

    // Delete the current add-on object and return back to the None state
    public void ResetAddOn()
    {
        if (currentState != AddOnCreationState.None)
        {
            Destroy(currentAddOnObject.gameObject);
            currentAddOnObject = null;
            currentState = AddOnCreationState.None;
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
