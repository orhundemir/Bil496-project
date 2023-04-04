using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSelectionManager : MonoBehaviour
{

    private List<GameObject> selectedWalls = new List<GameObject>();

    // If the mouse was clicked on a wall or one of its hinges, select or deselect it based on its current state
    public void HandleWallSelection()
    {
        GameObject wall = GetObjectByTagOnRaycastHit(Input.mousePosition, "Wall");
        GameObject hinge = GetObjectByTagOnRaycastHit(Input.mousePosition, "Hinge");

        if (wall != null || hinge != null)
        {
            GameObject selectedObject = wall != null ? wall.transform.parent.parent.gameObject : hinge.transform.parent.gameObject;

            if (selectedObject != null)
            {
                if (!selectedWalls.Contains(selectedObject))
                    SelectWall(selectedObject);
                else
                    DeselectWall(selectedObject);
            }
        }
    }

    public void SelectWall(GameObject wall)
    {
        wall.GetComponent<WallObject>().ToggleOutlines(true);
        selectedWalls.Add(wall);
    }

    public void DeselectWall(GameObject wall)
    {
        wall.GetComponent<WallObject>().ToggleOutlines(false);
        selectedWalls.Remove(wall);
    }

    public void RemoveSelectedWalls()
    {
        for (int i = selectedWalls.Count - 1; i >= 0; i--)
        {
            GameObject wall = selectedWalls[i];
            selectedWalls.Remove(wall);
            Destroy(wall);
        }
    }

    private GameObject GetObjectByTagOnRaycastHit(Vector3 position, string tag)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
            if (hit.collider.gameObject.CompareTag(tag))
                return hit.transform.gameObject;

        return null;
    }

    public List<GameObject> GetSelectedWalls()
    {
        return selectedWalls;
    }
}
