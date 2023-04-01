using System;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;

// Handles calls to the Google provider for authentication
public static class GoogleAuthenticator
{
    private const string ClientId = "432109852526-0n3757iplfa88qpjsvaidtql7lpcbctu.apps.googleusercontent.com"; //TODO: Change [CLIENT_ID] to your CLIENT_ID
    private const string ClientSecret = "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"; //TODO: Change [CLIENT_SECRET] to your CLIENT_SECRET

    private const int Port = 1234;
    private static readonly string RedirectUri = $"http://localhost:{Port}";
    
    private static readonly HttpCodeListener codeListener = new HttpCodeListener(Port);

    // Opens a webpage that prompts the user to sign in and copy the auth code     
    public static void GetAuthCode()
    {
        Application.OpenURL($"https://accounts.google.com/o/oauth2/v2/auth?client_id={ClientId}&redirect_uri={RedirectUri}&response_type=code&scope=email");

        codeListener.StartListening(code =>
        {
            ExchangeAuthCodeWithIdToken(code, idToken =>
            {
                FirebaseAuthHandler.SingInWithToken(idToken, "google.com");
            });
            
            codeListener.StopListening();
        });
    }
    
    // Exchanges the Auth Code with the user's Id Token
    public static void ExchangeAuthCodeWithIdToken(string code, Action<string> callback)
    {
        try
        {
            RestClient.Request(new RequestHelper
            {
                Method = "POST",
                Uri = "https://oauth2.googleapis.com/token",
                Params = new Dictionary<string, string>
                {
                    {"code", code},
                    {"client_id", ClientId},
                    {"client_secret", ClientSecret},
                    {"redirect_uri", RedirectUri},
                    {"grant_type", "authorization_code"}
                }
            }).Then(
                response =>
                {
                    var data =
                        StringSerializationAPI.Deserialize(typeof(GoogleIdTokenResponse), response.Text) as
                            GoogleIdTokenResponse;
                    callback(data.id_token);
                }).Catch(Debug.Log);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
