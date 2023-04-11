using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{

    public void DisplayTutorial(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        transform.GetChild(index).gameObject.SetActive(true);
    }

}
