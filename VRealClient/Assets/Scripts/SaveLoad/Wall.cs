using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wall {

    // wall/door/window = 0, ceiling/floor = 1
    public int type;

    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public string material;
    
    public override string ToString() {
        return "Position: " + position + " Rotation: " + rotation + " Scale " + scale + " Material " + material;
    }
}