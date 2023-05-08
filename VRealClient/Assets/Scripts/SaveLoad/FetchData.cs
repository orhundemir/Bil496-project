using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

public class FetchData : ScriptableObject
{

    /*
    private void Start() {
        List<string> requestedProducts = new List<string> {
            "sandsberg-table-black-s29420393",
            "bergpalm-duvet-cover-and-pillowcase-s-gray-stripe-30423239",
            "kungsbacka-door-anthracite-90337922",
            "haugesund-spring-mattress-medium-firm-dark-beige-80307416",
        };
        RunIkeaApi(requestedProducts);
    }
    */

    [SerializeField] private bool printResults = false;

    public List<IkeaProduct> RunIkeaApi(List<string> requestedProducts)
    {
        List<IkeaProduct> products = new(requestedProducts.Count);
        int batchSize = 5;
        for (int i = 0; i < requestedProducts.Count; i += batchSize)
        {
            // Get the next 10 elements from the requestedProducts list
            List<string> nextBatch = requestedProducts.GetRange(i, Math.Min(batchSize, requestedProducts.Count - i));
            List<IkeaProduct> batchResult = RunIkeaApiForNextBatch(nextBatch);
            products.AddRange(batchResult);
        }

        return products;
    }

    public List<IkeaProduct> RunIkeaApiForNextBatch(List<string> requestedProducts)
    {
        Process process = new Process();

        // Path to the Python interpreter, will require absolute path if python.exe is not added to the Environment Variables
        process.StartInfo.FileName = "python.exe";

        // Path to the Python file and IKEA product names passed as command line arguments
        string scriptPath = Path.Combine(Application.dataPath, "Scripts", "IKEA API", "IkeaProductScraper.py");
        process.StartInfo.Arguments = "\"" + scriptPath + "\" " + ($"{string.Join(" ", requestedProducts)}").Trim();

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

        // Get the output that was redirected from the Python script
        string output = process.StandardOutput.ReadToEnd();

        List<IkeaProduct> products = null;
        if (output != null)
        {
            products = JsonConvert.DeserializeObject<List<IkeaProduct>>(output);

            int itemCount = products == null ? 0 : products.Count;

            if (printResults && itemCount > 0)
            {
                UnityEngine.Debug.Log("Fetched " + itemCount + "/" + requestedProducts.Count + " items from IKEA");
                string itemDetails = "";
                foreach (IkeaProduct item in products)
                {
                    itemDetails += item + "\n\n";
                }
                UnityEngine.Debug.Log(itemDetails);
            }
        }

        return (products == null || products.Count == 0) ? null : products;
    }

}
