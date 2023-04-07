using System;

/// Response object to exchanging the Google Auth Code with the Id Token

[Serializable]
public class GoogleIdTokenResponse
{
    public string id_token;
}
