using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHandler : MonoBehaviour
{

    public GameObject wallPrefab;
    private Transform parentObject;

    private void Start()
    {
        parentObject = GameObject.Find("Walls").transform;
    }

    // Create a Wall at the given position and add it to the game
    public GameObject CreateWall(Vector3 position)
    {
        return Instantiate(wallPrefab, position, Quaternion.identity, parentObject);
    }

    // Update the given WallObjects data
    public WallObject UpdateWall(WallObject wall, Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        if (distance > 1)
        {
            direction.Normalize();

            Quaternion rotation = Quaternion.LookRotation(direction);
            wall.transform.rotation = rotation;

            Vector3 position = start + direction * distance / 2;
            wall.transform.position = position;

            Vector3 scale = new Vector3(wall.GetWidth(), wall.GetHeight(), distance);
            wall.transform.localScale = scale;
        }

        return wall;
    }

    public void RemoveWall(GameObject wall)
    {
        Destroy(wall);
    }

    public void RemoveWall(WallObject wall)
    {
        Destroy(wall);
    }

}
