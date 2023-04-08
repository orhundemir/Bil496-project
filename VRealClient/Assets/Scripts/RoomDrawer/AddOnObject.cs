using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnObject : MonoBehaviour
{

    public Material finalMaterial, validMaterial, invalidMaterial, realisticMaterial;

    private GameObject addOn;
    private float addOnLength;

    private void Awake()
    {
        addOn = transform.GetChild(0).gameObject;

        if (addOn.CompareTag("Door"))
            addOnLength = 1.5f;
        else if (addOn.CompareTag("Window"))
            addOnLength = 1.8f;
    }

    public void ChangeMaterialToFinal()
    {
        addOn.GetComponent<Renderer>().material = finalMaterial;
    }

    public void ChangeMaterialToInvalid()
    {
        addOn.GetComponent<Renderer>().material = invalidMaterial;
    }

    public void ChangeMaterialToValid()
    {
        addOn.GetComponent<Renderer>().material = validMaterial;
    }

    public void ChangeMaterialToRealistic()
    {
        addOn.GetComponent<Renderer>().material = realisticMaterial;
    }

    public float GetLength()
    {
        return addOnLength;
    }
}
