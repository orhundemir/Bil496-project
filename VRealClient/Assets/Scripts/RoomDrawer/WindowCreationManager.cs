using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowCreationManager : MonoBehaviour
{

    public GameObject windowPrefab;

    private WindowObject windowObject = null;

    public void StartWindowCreation(Vector3 position)
    {
        GameObject window = CreateWindow(position);

        windowObject = window.GetComponent<WindowObject>();
        windowObject.ChangeMaterialToInvalid();

        window.transform.localScale = new Vector3(.6f, 0.1f, windowObject.GetLength());
    }

    public void UpdateWindow(Vector3 mousePosition)
    {
        GameObject wall = GetObjectByTagOnRaycastHit(Input.mousePosition, "Wall");

        if (wall == null)
        {
            windowObject.ChangeMaterialToInvalid();

            windowObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
            windowObject.transform.position = new Vector3(mousePosition.x, 0f, mousePosition.z);

            windowObject.transform.SetParent(transform);
        }
        else
        {
            windowObject.ChangeMaterialToValid();

            WallObject wallObject = wall.transform.parent.parent.GetComponent<WallObject>();
            Vector3 wallStartPosition = wallObject.hinge1.transform.position;
            Vector3 wallEndPosition = wallObject.hinge2.transform.position;

            Vector3 wallVector = wallEndPosition - wallStartPosition;
            Vector3 mouseVector = mousePosition - wallStartPosition;
            Vector3 projection = Vector3.Project(mouseVector, wallVector);

            windowObject.transform.SetParent(wall.transform.parent.parent);
            windowObject.transform.position = wallStartPosition + projection + new Vector3(0, 0.05f, 0);
            windowObject.transform.rotation = wall.transform.rotation;
        }
    }

    public void PlaceWindow(Vector3 mousePosition)
    {

    }

    private GameObject CreateWindow(Vector3 position)
    {
        return Instantiate(windowPrefab, position, Quaternion.Euler(0, 90f, 0), transform);
    }

    private GameObject GetObjectByTagOnRaycastHit(Vector3 position, string tag)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
            if (hit.collider.gameObject.CompareTag(tag))
                return hit.transform.gameObject;

        return null;
    }

    public bool WindowObjectExists()
    {
        return windowObject != null;
    }

}
