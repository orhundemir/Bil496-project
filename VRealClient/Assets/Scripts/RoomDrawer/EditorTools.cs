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

    // Onclick methods for the tools menu
    public void SelectSelectorTool()
    {
        Debug.Log("SELECTOR");
        selectedTool = TOOLS.SELECTOR;
    }

    public void SelectWallTool()
    {
        Debug.Log("WALL");
        selectedTool = TOOLS.WALL;
    }

    public void SelectDoorTool()
    {
        Debug.Log("DOOR");
        selectedTool = TOOLS.DOOR;
    }

    public void SelectWindowTool()
    {
        Debug.Log("WINDOW");
        selectedTool = TOOLS.WINDOW;
    }

    // All code below this line is temporary, only for testing purposes
    private void Update()
    {
        ChangeTools();
    }

    private void ChangeTools()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSelectorTool();
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SelectWallTool();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SelectWindowTool();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SelectDoorTool();
        }
    }

}
