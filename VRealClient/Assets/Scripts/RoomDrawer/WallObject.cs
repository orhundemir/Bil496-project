using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour 
{

    public Material wallMaterial, previewMaterial, selectedMaterial;

    private GameObject wall, hinge1, hinge2;
    public Material hingeMaterial, hingeHoverMaterial;

    private float wallWidth = .6f, wallHeight = 0.1f;
    private float hingeWidth, hingeScaleFactor = 1.3f;

    private void Awake()
    {
        hingeWidth = wallWidth * hingeScaleFactor;

        wall = transform.GetChild(0).GetChild(0).gameObject;
        hinge1 = transform.GetChild(1).gameObject;
        hinge2 = transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        CheckHingeHover();
    }

    // Changes the hinge's material if the mouse is hovering over it, providing visual feedback for the user
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

    public void ActivateHinges()
    {
        hinge1.SetActive(true);
        hinge2.SetActive(true);
    }

    // Move the hinges to opposite ends of the wall
    public void AdjustHingePositions(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        direction.Normalize();

        hinge1.transform.position = start;
        hinge2.transform.position = start + direction * distance;

        hinge1.transform.localScale = new Vector3(hingeWidth, wallHeight, hingeWidth);
        hinge2.transform.localScale = new Vector3(hingeWidth, wallHeight, hingeWidth);
    }

    public void ChangeWallMaterialToTransparent()
    {
        wall.GetComponent<Renderer>().material = previewMaterial;
    }

    public void ChangeWallMaterialToChosenTexture()
    {
        wall.GetComponent<Renderer>().material = wallMaterial;
    }

    public void ChangeWallMaterialToSelected()
    {
        wall.GetComponent<Renderer>().material = selectedMaterial;
    }

    public void SetChosenTextureMaterial(Material material)
    {
        wallMaterial = material;
    }

    public float GetWidth()
    {
        return wallWidth;
    }

    public float GetHeight()
    {
        return wallHeight;
    }

}
