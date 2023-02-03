using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Json objects sent from the Python script will be deserialized into objects of this class
[System.Serializable]
public class IkeaProduct {

    public string productName;
    public string productType;
    public string productPrice;
    public string productImageURL;
    public string productMeasurements;

    public override string ToString() {
        return "name: " + productName + "\ntype: " + productType + "\nprice: " + productPrice + "\nimage: " + productImageURL + "\nmeasurements: " + productMeasurements;
    }

}
