using System;
using UnityEngine;
using UnityEngine.UI;

public class WallUIManager : MonoBehaviour
{

    public GameObject wallInfoCanvas;
    public Text wallLengthText, wallAngleText;

    public void SetCanvasPosition(Vector3 position)
    {
        wallInfoCanvas.transform.position = position;
    }

    public void SetLengthTextPosition(Vector3 position)
    {
        wallLengthText.transform.position = position;
    }

    public void SetAngleValue(int angle)
    {
        if (angle < 0)
            angle += 360;
        wallAngleText.text = angle + " " + (char) 176;
    }

    public void SetLengthValue(double length, int roundDigits)
    {
        wallLengthText.text = Math.Round(length, roundDigits) + " m";
    }

    public void SetActive(bool active)
    {
        wallInfoCanvas.SetActive(active);
    }

}
