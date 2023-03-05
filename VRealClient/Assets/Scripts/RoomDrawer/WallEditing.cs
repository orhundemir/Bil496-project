using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEditing : MonoBehaviour
{
    public WallSelectionManager wallSelectionManager;
    public FlexibleColorPicker colorPicker;

    public Material defaultWallMaterial;

    List<GameObject> selectedWalls = new List<GameObject>();

    private void Start()
    {
        colorPicker.SetColor(defaultWallMaterial.color);
    }

    public void ChangeWallpaper(Material selectedMaterial)
    {
        selectedWalls = wallSelectionManager.GetSelectedWalls();
        foreach(GameObject selectedWall in selectedWalls)
        {
            selectedWall.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = selectedMaterial;
            selectedWall.GetComponent<WallObject>().SetChosenTextureMaterial(selectedMaterial);
        }
    }

    public void ChangeWallColour(Material selectedMaterial)
    {
        Material selectedColor = new Material(selectedMaterial);
        selectedColor.color = colorPicker.color;
        selectedWalls = wallSelectionManager.GetSelectedWalls();
        foreach (GameObject selectedWall in selectedWalls)
        {
            selectedWall.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = selectedColor;
            selectedWall.GetComponent<WallObject>().SetChosenTextureMaterial(selectedColor);
        }
    }

    public void CompleteEditing()
    {
        wallSelectionManager.GetSelectedWalls().Clear();
    }
}
