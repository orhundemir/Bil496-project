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
            windowObject.transform.position = new Vector3(mousePosition.x, 0f, mousePosition.z);
            windowObject.transform.rotation = Quaternion.Euler(0, 90f, 0);
        }
        else
        {
            windowObject.transform.rotation = wall.transform.rotation;
            windowObject.transform.position = new Vector3(mousePosition.x, 0f, mousePosition.z);
            //windowObject.transform.position = wall.transform.position * -0.5f;
            //float projection = Vector3.Dot(mouseDirection, wallDirection);
            //Vector3 wallCenterPoint = wallStart + wallDirection * projection; ;
        }
    }

    public void PlaceWindow(Vector3 mousePosition)
    {

    }

    private GameObject CreateWindow(Vector3 position)
    {
        return Instantiate(windowPrefab, position, Quaternion.Euler(0, 90f, 0));
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
