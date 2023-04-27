using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

public class IkeaCartManager : MonoBehaviour
{

    // Called from the on-click of the View Cart Button
    public void AddFurnituresToIkeaCart()
    {
        List<string> productCodes = RetrieveProductCodes();
        RunCartEndpoint(productCodes);
    }

    // Runs the IKEA API to add the given products to the guest user's cart
    // Automatically redirects to the IKEA website from Python
    private void RunCartEndpoint(List<string> productCodes)
    {
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

    private List<string> RetrieveProductCodes()
    {
        int furnitureCount = transform.childCount;
        List<string> productCodes = new List<string>(furnitureCount);

        for (int i = 0; i < furnitureCount; i++)
        {
            string productName = transform.GetChild(i).name;
            productCodes.Add(ExtractProductCode(productName));
        }

        return productCodes;
    }

    private string ExtractProductCode(string productName)
    {
        string code = productName.Substring(productName.LastIndexOf("-") + 1);

        int startIndex = GetFirstDigitIndex(code);
        if (startIndex != -1)
            code = code.Substring(startIndex);
        if (code.Contains("("))
            code = code.Substring(0, code.IndexOf("("));

        return code;
    }

    private int GetFirstDigitIndex(string str)
    {
        for (int i = 0; i < str.Length; i++)
            if (Char.IsDigit(str[i]))            
                return i;
        return -1;
    }

}
