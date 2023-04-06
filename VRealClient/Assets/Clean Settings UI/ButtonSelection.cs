using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour
{

    private Color defaultButtonColor = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
    private Color selectedButtonColor = new Color(113.0f / 255.0f, 171.0f / 255.0f, 202.0f / 255.0f);
    private Color defaultTextColor = new Color(77.0f / 255.0f, 83.0f / 255.0f, 91.0f / 255.0f);
    private Color selectedTextColor = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);

    public void HandleSelectedButtonUI(int buttonIndex)
    {
        GameObject selectedButton = transform.GetChild(buttonIndex).gameObject;
        selectedButton.GetComponent<Image>().color = selectedButtonColor;
        selectedButton.transform.GetChild(0).GetComponent<Text>().color = selectedTextColor;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == buttonIndex)
                continue;

            GameObject button = transform.GetChild(i).gameObject;
            button.GetComponent<Image>().color = defaultButtonColor;
            button.transform.GetChild(0).GetComponent<Text>().color = defaultTextColor;
        }
    }

}
