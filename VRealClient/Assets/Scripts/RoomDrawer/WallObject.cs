using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour {

    private new Renderer renderer;
    public Material wallMaterial, previewMaterial;

    private int wallWidth = 10, wallHeight = 1;

    public void ChangeMaterialToTransparent() {
        if (renderer == null)
            renderer = transform.GetChild(0).GetComponent<Renderer>();

        renderer.material = previewMaterial;
    }

    public void ChangeMaterialToOpaque() {
        if (renderer == null)
            renderer = transform.GetChild(0).GetComponent<Renderer>();

        renderer.material = wallMaterial;
    }

    public int GetWidth() {
        return wallWidth;
    }

    public int GetHeight() {
        return wallHeight;
    }

}
