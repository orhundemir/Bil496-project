using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WallHandler : MonoBehaviour
{

    public GameObject wallPrefab;
    public float lengthRatio;

    // Create a Wall at the given position and add it to the game
    public GameObject CreateWall(Vector3 position)
    {
        return Instantiate(wallPrefab, position, Quaternion.identity, transform);
    }

    // Update the given WallObject's transform values
    public void UpdateWall(WallObject wallObject, Vector3 start, Vector3 end, Text wallInfoText)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        if (distance > 1)
        {
            direction.Normalize();

            Quaternion rotation = Quaternion.LookRotation(direction);

            wallObject.transform.rotation = rotation;

            int angleValue = (int)rotation.eulerAngles.y;

            Vector3 scale = new Vector3(wallObject.GetWidth(), wallObject.GetHeight(), distance);
            wallObject.transform.GetChild(0).localScale = scale;
            GameObject hinge1 = wallObject.transform.GetChild(1).gameObject;
            GameObject hinge2 = wallObject.transform.GetChild(2).gameObject;

            hinge1.transform.position = start;
            hinge2.transform.position = start + direction * distance;

            wallInfoText.transform.position = start + (direction * distance) / 2;
            wallInfoText.text = Math.Round((wallObject.transform.GetChild(0).localScale.z / lengthRatio), 1).ToString() + "  m";
        }
    }

    public void ActivateHinges(WallObject wallObject)
    {
        GameObject hinge1 = wallObject.transform.GetChild(1).gameObject;
        GameObject hinge2 = wallObject.transform.GetChild(2).gameObject;
        hinge1.SetActive(true);
        hinge2.SetActive(true);
    }

    public void RemoveWall(GameObject wall)
    {
        Destroy(wall);
    }

}