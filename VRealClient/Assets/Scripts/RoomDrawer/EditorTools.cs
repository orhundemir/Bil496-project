using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTools : MonoBehaviour
{
    
    public enum TOOLS
    {
        SELECTOR,
        WALL,
        WINDOW,
        DOOR
    }
    public static TOOLS selectedTool = TOOLS.WALL;

    // All code below this line is temporary, only for testing purposes
    private void Update()
    {
        ChangeTools();
    }

    private void ChangeTools()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTool = TOOLS.SELECTOR;
            Debug.Log("SELECTOR");
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            selectedTool = TOOLS.WALL;
            Debug.Log("WALL");
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            selectedTool = TOOLS.WINDOW;
            Debug.Log("WINDOW");
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            selectedTool = TOOLS.DOOR;
            Debug.Log("DOOR");
        }
    }

}
