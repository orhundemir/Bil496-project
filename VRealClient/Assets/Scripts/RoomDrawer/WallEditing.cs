using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEditing : MonoBehaviour
{
    public WallSelectionManager wallSelectionManager;

    List<GameObject> selectedWalls = new List<GameObject>();

    public void ChangeWallpaper(Material selectedMaterial)
    {
        selectedWalls = wallSelectionManager.GetSelectedWalls();
        foreach(GameObject selectedWall in selectedWalls)
        {
            selectedWall.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = selectedMaterial;

        }
    }

    public void CompleteEditing()
    {
        wallSelectionManager.GetSelectedWalls().Clear();

    }
}
