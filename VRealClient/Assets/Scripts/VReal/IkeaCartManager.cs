using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

public class IkeaCartManager : MonoBehaviour
{

    
    public void Start()
    {
        List<string> productNames = new List<string>
        {
            "uppland-cover-for-armchair-virestad-red-white-30472729",
            "uppland-cover-for-armchair-virestad-red-white-30472729",
            "uppland-cover-for-armchair-virestad-red-white-30472729",
            "uppland-cover-for-armchair-virestad-red-white-30472729",
            "tullsta-armchair-lofallet-beige-s89272714",
            "tullsta-armchair-lofallet-beige-s89272714",
            "ingolf-chair-white-70103250",
            "test-for-exception-123abc123"
        };
        AddItemsToIkeaCart(productNames);    
    }

    public void AddItemsToIkeaCart(List<string> productNames)
    {
        List<string> productCodes = ExtractItemCodes(productNames);
        
        foreach (string a in productCodes)
            UnityEngine.Debug.Log(a);
        Process process = new Process();

        // Path to the Python interpreter, will require absolute path if python.exe is not added to the Environment Variables
        process.StartInfo.FileName = "python.exe";

        // Path to the Python file and IKEA product names passed as command line arguments
        process.StartInfo.Arguments = "\"" + Directory.GetCurrentDirectory() + 
            @"\Assets\Scripts\IKEA API\IkeaCartManager.py" +
            "\" " + $"{string.Join(" ", productCodes)}";

        // Redirect the outputs and errors from Python to Unity
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        // Run the Python script
        process.Start();
        process.WaitForExit();

        string error = process.StandardError.ReadToEnd();
        if (error != null && error != "")
            UnityEngine.Debug.LogError(error);
        else
            UnityEngine.Debug.Log("Cart access was successful");
    }

    private List<string> ExtractItemCodes(List<string> productNames)
    {
        List<string> productCodes = new List<string>(productNames.Count);
        foreach (string productName in productNames)
        {
            string code = productName.Substring(productName.LastIndexOf("-") + 1);

            int startIndex = GetFirstDigitIndex(code);
            if (startIndex != -1)
                productCodes.Add(code.Substring(startIndex));
        }
        
        return productCodes;
    }

    private int GetFirstDigitIndex(string str)
    {
        for (int i = 0; i < str.Length; i++)
            if (Char.IsDigit(str[i]))            
                return i;
        return -1;
    }

}
