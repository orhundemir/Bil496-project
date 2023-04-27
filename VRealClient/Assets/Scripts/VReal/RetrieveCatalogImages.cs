using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RetrieveCatalogImages : MonoBehaviour
{
    [SerializeField] private Transform categoryPanels;
    [SerializeField] private List<Button> productButtons;
    
    // The button list will be matched with their product URL names in this dictionary
    private Dictionary<string, Button> buttonImageMap = new();

    private void Awake()
    {
        // Create the product name list needed for the API call
        List<string> productNames = new();
        foreach (Button button in productButtons)
        {
            productNames.Add(button.name);
            buttonImageMap.Add(button.name, button);
        }

        // Retrieve the needed data for each product from the IKEA API
        FetchData fetchData = ScriptableObject.CreateInstance<FetchData>();
        AssignImages(fetchData.RunIkeaApi(productNames));
    }

    private void AssignImages(List<IkeaProduct> productData)
    {
        // Assign images to buttons
        foreach (IkeaProduct product in productData)
        {
            Button button = buttonImageMap[product.productId];
            buttonImageMap.Remove(product.productId);

            if (product.productImageURL == null || product.productImageURL.Equals(""))
            {
                StartCoroutine(LoadImage(button, "https://eagle-sensors.com/wp-content/uploads/unavailable-image.jpg"));
                UpdateTextArea(button, "Unavailable", "Unavailable");
            }
            else
            {
                StartCoroutine(LoadImage(button, product.productImageURL));
                UpdateTextArea(button, product.productName, product.productPrice);
            }
        }

        // Assign default image to not found products
        foreach (var mapEntry in buttonImageMap)
        {
            StartCoroutine(LoadImage(mapEntry.Value, "https://eagle-sensors.com/wp-content/uploads/unavailable-image.jpg"));
            UpdateTextArea(mapEntry.Value, "Unavailable", "Unavailable");
        }
    }

    private void UpdateTextArea(Button button, string productName, string productPrice)
    {
        button.transform.GetChild(0).GetComponent<Text>().text = productName + "\n" + productPrice;
    }

    private IEnumerator LoadImage(Button button, string productImageURL)
    {
        UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(productImageURL);
        yield return unityWebRequest.SendWebRequest();

        Texture2D texture = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        button.image.sprite = sprite;
    }

}
