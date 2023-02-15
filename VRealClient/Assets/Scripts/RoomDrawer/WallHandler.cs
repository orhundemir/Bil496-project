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

    // Update the given WallObject's transform values
    public void UpdateWall(WallObject wallObject, Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        if (distance > 1)
        {
            direction.Normalize();

            Quaternion rotation = Quaternion.LookRotation(direction);
            wallObject.transform.rotation = rotation;

            Vector3 position = start + direction * distance / 2;
            wallObject.transform.position = position;

            Vector3 scale = new Vector3(wallObject.GetWidth(), wallObject.GetHeight(), distance);
            wallObject.transform.localScale = scale;
        }
    }

    // Activate the hinges, transform and scale them so that they are positioned at the opposite ends of the wall
    public void AdjustHinges(WallObject wallObject) 
    {
        GameObject hinge1 = wallObject.transform.GetChild(1).gameObject;
        GameObject hinge2 = wallObject.transform.GetChild(2).gameObject;

        // Inverse the scaling of the hinges to reset their scaling and rescale their radius to make them wider than the wall
        Vector3 wallObjectScale = wallObject.transform.localScale;
        float widthScale = wallObjectScale.x * 1.3f;
        hinge1.transform.localScale = new Vector3(widthScale / wallObjectScale.x, 1 / wallObjectScale.y, widthScale / wallObjectScale.z);
        hinge2.transform.localScale = new Vector3(widthScale / wallObjectScale.x, 1 / wallObjectScale.y, widthScale / wallObjectScale.z);

        // Move the hinges to the opposite ends of the wall
        hinge1.transform.localPosition = new Vector3(0, 0, -0.5f);
        hinge2.transform.localPosition = new Vector3(0, 0, 0.5f);

        hinge1.SetActive(true);
        hinge2.SetActive(true);
    }

    public void RemoveWall(GameObject wall)
    {
        Destroy(wall);
    }

}
