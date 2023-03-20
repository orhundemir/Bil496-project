using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class getImage : MonoBehaviour
{
    [SerializeField]private Image image;

    // Start is called before the first frame update
    void Start()
    {
        FetchData fetchData = new FetchData();
        List<string> productToFetch = new List<string>();
        productToFetch.Add(this.name);
        List<IkeaProduct> products = fetchData.RunIkeaApi(productToFetch);

        _ = products == null 
                ? StartCoroutine(LoadImage("https://eagle-sensors.com/wp-content/uploads/unavailable-image.jpg")) 
                : StartCoroutine(LoadImage(products[0].productImageURL));
    }

    private IEnumerator LoadImage(string productImageURL)
    {        
        UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(productImageURL);
        yield return unityWebRequest.SendWebRequest();

        Texture2D texture = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }
     

}
