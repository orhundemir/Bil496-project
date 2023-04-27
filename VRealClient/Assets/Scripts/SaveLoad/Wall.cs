using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wall{
    //Hinge1 coordinates
    public float x1;
    public float y1;
    public float z1;
    //Hinge1 coordinates
    public float x2;
    public float y2;
    public float z2;
    //Wall metarial
    public string wallMaterial;
    
    public string tostring(){
        return "Hinge1 coordinates: "+x1+", "+y1+", "+z1+" Hinge2 coordinates: "+x2+", "+y2+", "+z2+" Wall material: "+wallMaterial;
    }
}