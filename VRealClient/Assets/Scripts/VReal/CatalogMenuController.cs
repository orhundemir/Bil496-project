using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatalogMenuController : MonoBehaviour
{

    [SerializeField] private GameObject categoryPanels, categoryButtons;

    private Color defaultColor = new Color(1f, 1f, 1f);
    private Color selectedColor = new Color(186f / 255, 186f / 255, 186f / 255);

    // Deactivate all category panels except the panel with the given index
    public void HandleCategorySelection(int index)
    {
        for (int i = 0; i < categoryPanels.transform.childCount; i++)
        {
            categoryPanels.transform.GetChild(i).gameObject.SetActive(false);
            categoryButtons.transform.GetChild(i).GetComponent<Image>().color = defaultColor;
        }
        categoryButtons.transform.GetChild(index).GetComponent<Image>().color = selectedColor;
        categoryPanels.transform.GetChild(index).gameObject.SetActive(true);
    }

}
