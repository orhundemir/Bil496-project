using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallHandler : MonoBehaviour
{

    public GameObject wallPrefab;
    private List<GameObject> selectedWalls = new List<GameObject>();

    public float lenghRatio = 1;

    // Create a Wall at the given position and add it to the game
    public GameObject CreateWall(Vector3 position)
    {
        return Instantiate(wallPrefab, position, Quaternion.identity, transform);
    }

    // Update the given WallObject's transform values so that it is drawn between the given start and end vectors
    // Also update its displayed length and angle values
    public void UpdateWall(WallObject wallObject, Vector3 start, Vector3 end, bool limitAngles, Text wallInfoText, Text angleText)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        if (distance > 1)
        {
            direction.Normalize();

            // Round the angle of rotation to the nearest integer and rotate the wall object accordingly
            Quaternion rotation = Quaternion.LookRotation(direction);
            int angle = Mathf.RoundToInt(rotation.eulerAngles.y);
            if (limitAngles)
            {
                rotation.eulerAngles = new Vector3(0, angle, 0);
                direction = Quaternion.Euler(0, -90, 0) * rotation * Vector3.right;
            }
            wallObject.transform.rotation = rotation;

            Vector3 scale = new Vector3(wallObject.GetWidth(), wallObject.GetHeight(), distance);
            wallObject.transform.GetChild(0).localScale = scale;

            // Move the hinges at both ends of the wall
            GameObject hinge1 = wallObject.transform.GetChild(1).gameObject;
            GameObject hinge2 = wallObject.transform.GetChild(2).gameObject;
            hinge1.transform.position = start;
            hinge2.transform.position = start + direction * distance;

            angle -= 90;
            if (angle < 0)
                angle += 360;
            angleText.text = angle + " " + (char) 176;

            wallInfoText.transform.position = start + (direction * distance) / 2;
            // Move wallInfoText up in the +y direction so that it appears above other wall objects
            wallInfoText.transform.position += new Vector3(0, 0.5f, 0);
            wallInfoText.text = Math.Round((wallObject.transform.GetChild(0).localScale.z / lenghRatio), 1).ToString() + " m";
        }
    }

    public void ActivateHinges(WallObject wallObject)
    {
        GameObject hinge1 = wallObject.transform.GetChild(1).gameObject;
        GameObject hinge2 = wallObject.transform.GetChild(2).gameObject;
        hinge1.SetActive(true);
        hinge2.SetActive(true);
    }

    public void SelectWall(GameObject wall)
    {
        selectedWalls.Add(wall);
        wall.GetComponent<WallObject>().ChangeWallMaterialToSelected();
    }

    public void RemoveWall(GameObject wall)
    {
        Destroy(wall);
    }

}