using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowObject : MonoBehaviour
{

    public Material windowMaterial, validWindowMaterial, invalidWindowMaterial;

    private GameObject window;
    private float windowLength = 2f;

    private void Awake()
    {
        window = transform.GetChild(0).gameObject;
    }

    public void ChangeMaterialToWindow()
    {
        window.GetComponent<Renderer>().material = windowMaterial;
    }
    public void ChangeMaterialToInvalid()
    {
        window.GetComponent<Renderer>().material = invalidWindowMaterial;
    }

    public void ChangeMaterialToValid()
    {
        window.GetComponent<Renderer>().material = validWindowMaterial;
    }

    public float GetLength()
    {
        return windowLength;
    }

}
