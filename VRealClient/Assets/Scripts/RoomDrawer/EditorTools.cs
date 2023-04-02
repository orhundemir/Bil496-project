using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTools : MonoBehaviour
{
    
    public enum Tool
    {
        Selector,
        Wall,
        Window,
        Door
    }
    public static Tool SelectedTool { get; private set; } = Tool.Wall;

    // Onclick methods for the tools menu
    public void SelectSelectorTool()
    {
        SelectedTool = Tool.Selector;
    }

    public void SelectWallTool()
    {
        SelectedTool = Tool.Wall;
    }

    public void SelectDoorTool()
    {
        SelectedTool = Tool.Door;
    }

    public void SelectWindowTool()
    {
        SelectedTool = Tool.Window;
    }

}
