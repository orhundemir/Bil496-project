using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallHandler : MonoBehaviour
{

    public GameObject wallPrefab;

    public float lenghRatio = 1;

    // Create a Wall at the given position and add it to the game
    public GameObject CreateWall(Vector3 position)
    {
        return Instantiate(wallPrefab, position, Quaternion.identity, transform);
    }

    // Update the given WallObject's transform values so that it is drawn between the given start and end vectors
    public void UpdateWall(WallObject wallObject, Vector3 start, Vector3 end, bool limitAngles, out Vector3 centerPosition, out int wallAngle, out double wallLength)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        centerPosition = new Vector3();
        wallAngle = 0;
        wallLength = 0;

        if (distance > 0)
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

            // Calculate the output values, they will be passed into WallUIManager later
            wallAngle = angle - 90;
            centerPosition = start + (direction * distance) / 2 + new Vector3(0, 0.5f, 0);
            wallLength = wallObject.transform.GetChild(0).localScale.z / lenghRatio;
        }
    }

    public void UpdateWall(WallObject wallObject, Vector3 start, Vector3 end, bool limitAngles)
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
        }
    }

    public void ActivateHinges(WallObject wallObject)
    {
        GameObject hinge1 = wallObject.transform.GetChild(1).gameObject;
        GameObject hinge2 = wallObject.transform.GetChild(2).gameObject;
        hinge1.SetActive(true);
        hinge2.SetActive(true);
    }

}