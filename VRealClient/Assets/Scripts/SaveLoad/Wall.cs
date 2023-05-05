using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wall {

    /*
     * Wall    - 0
     * Window  - 1
     * Door    - 2
     * Ceiling - 3
     * Floor   - 4
     */
    public int type;

    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public string material;
    public Color color;
    
    public override string ToString() {
        return "Position: " + position + " Rotation: " + rotation + " Scale " + scale + " Material " + material;
    }
}