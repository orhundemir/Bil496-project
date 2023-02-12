using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHandler : MonoBehaviour {

    public GameObject wallPrefab;

    // Create a new wall between the given points and add it to the scene
    public void AddWall(Vector3 start, Vector3 end) {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        if (distance > 1) {
            direction.Normalize();

            Quaternion rotation = Quaternion.LookRotation(direction);
            Vector3 position = start + direction * distance / 2;
            // Add the new wall to the scene under the Walls object
            GameObject wall = Instantiate(wallPrefab, position, rotation, GameObject.Find("Walls").transform);

            int wallWidth = 5;
            int wallHeight = 1;
            Vector3 scale = new Vector3(wallWidth, wallHeight, distance);
            wall.transform.localScale = scale;
        }
    }

    public void RemoveWall(GameObject wall) {
        Destroy(wall);
    }

}
