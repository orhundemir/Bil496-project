using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSelectionManager : MonoBehaviour
{

    private List<GameObject> selectedWalls = new List<GameObject>();

    // If the mouse was clicked on a wall, select or deselect it based on its current state
    public void HandleWallSelection()
    {
        GameObject wall = GetObjectByTagOnRaycastHit(Input.mousePosition, "Wall");
        if (wall != null)
        {
            wall = wall.transform.parent.parent.gameObject;

            if (!selectedWalls.Contains(wall))
                SelectWall(wall);
            else
                DeselectWall(wall);
        }
    }

    private void SelectWall(GameObject wall)
    {
        wall.GetComponent<WallObject>().ChangeWallMaterialToSelected();
        selectedWalls.Add(wall);
    }

    private void DeselectWall(GameObject wall)
    {
        wall.GetComponent<WallObject>().ChangeWallMaterialToOpaque();
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

}
