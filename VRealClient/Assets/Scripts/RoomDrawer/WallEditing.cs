using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEditing : MonoBehaviour
{
    public WallSelectionManager wallSelectionManager;
    public FlexibleColorPicker colorPicker;

    public GameObject Ground;

    public Material defaultWallMaterial;


    private void Start()
    {
        colorPicker.SetColor(defaultWallMaterial.color);
    }

    public void ChangeWallpaper(Material selectedMaterial)
    {
        List<GameObject> selectedWalls = wallSelectionManager.GetSelectedWalls();
        foreach(GameObject selectedWall in selectedWalls)
        {
            selectedWall.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = selectedMaterial;
            selectedWall.GetComponent<WallObject>().SetChosenTextureMaterial(selectedMaterial);
        }
    }

    public void ChangeWallColour()
    {
        Material selectedColor = new Material(defaultWallMaterial);
        selectedColor.color = colorPicker.color;
        List<GameObject> selectedWalls = wallSelectionManager.GetSelectedWalls();
        foreach (GameObject selectedWall in selectedWalls)
        {
            selectedWall.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = selectedColor;
            selectedWall.GetComponent<WallObject>().SetChosenTextureMaterial(selectedColor);
        }
    }

    public void CompleteEditing()
    {
        List<GameObject> selectedWalls = wallSelectionManager.GetSelectedWalls();
        for (int i = selectedWalls.Count - 1; i >= 0; i--)
        {
            GameObject wall = selectedWalls[i];
            wallSelectionManager.DeselectWall(wall);
        }
    }

    public void ChangeGroundTexture(Material selectedMaterial)
    {
        Ground.GetComponent<MeshRenderer>().material = selectedMaterial; 
    }
}
