using Proyecto26;
using UnityEngine;
using Newtonsoft.Json.Linq;

//Handles authentication calls to Firebase
public static class FirebaseAuthHandler
{
    private const string ApiKey = "AIzaSyDxi3M7FiiH1e3ukbcODH0_xXkRXCPZZ8U"; 

    public static string userId { get; private set; }
    public static string email { get; private set; }
    public static string name { get; private set; }
    public static bool verified { get; private set; }
    
    public static void SingInWithToken(string token, string providerId)
    {
        var payLoad =
            $"{{\"postBody\":\"id_token={token}&providerId={providerId}\",\"requestUri\":\"http://localhost\",\"returnIdpCredential\":true,\"returnSecureToken\":true}}";
        RestClient.Post($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={ApiKey}", payLoad).Then(
            response =>
            {
                JObject data = JObject.Parse(response.Text);

                //Debug.Log(data);
                userId = (string)data["localId"];
                email = (string)data["email"];
                verified = (bool)data["emailVerified"];
                //name = (string)data["displayName"];

            }).Catch(Debug.Log);
    }
}
