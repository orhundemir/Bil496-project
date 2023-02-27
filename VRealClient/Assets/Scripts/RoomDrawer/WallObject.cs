using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour {

    public Material wallMaterial, previewMaterial, selectedMaterial;

    private GameObject wall, hinge1, hinge2;
    public Material hingeMaterial, hingeHoverMaterial;

    private readonly int wallWidth = 10, wallHeight = 1;

    private void Awake()
    {
        wall = transform.GetChild(0).GetChild(0).gameObject;
        hinge1 = transform.GetChild(1).gameObject;
        hinge2 = transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        CheckHingeHover();
    }

    // Change hinges material if the mouse is hovering over it
    private void CheckHingeHover()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.gameObject == hinge1 || hit.collider.gameObject == hinge2)
            {
                hit.collider.gameObject.GetComponent<Renderer>().material = hingeHoverMaterial;
            }
            else
            {
                hinge1.GetComponent<Renderer>().material = hingeMaterial;
                hinge2.GetComponent<Renderer>().material = hingeMaterial;
            }
        }
    }

    public void ChangeWallMaterialToTransparent()
    {
        wall.GetComponent<Renderer>().material = previewMaterial;
    }

    public void ChangeWallMaterialToOpaque()
    {
        wall.GetComponent<Renderer>().material = wallMaterial;
    }

    public void ChangeWallMaterialToSelected()
    {
        wall.GetComponent<Renderer>().material = selectedMaterial;
    }

    public int GetWidth()
    {
        return wallWidth;
    }

    public int GetHeight()
    {
        return wallHeight;
    }

}
