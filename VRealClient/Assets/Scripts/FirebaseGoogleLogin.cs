
using Firebase.Extensions;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseGoogleLogin : MonoBehaviour
{
    [SerializeField] Text email;
    [SerializeField] Text tokenID;
    [SerializeField] Text displayName;
   
    private string GoogleWebAPI = "932274708876-oti9g219e48sa36p1l5ir6s2q62and1l.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;
    Firebase.Auth.FirebaseAuth auth;
    public static Firebase.Auth.FirebaseUser user;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleWebAPI,
            RequestIdToken = true
        };
    }

    private void Start()
    {
        InitFirebase();
        if(auth.CurrentUser != null)
        {
            user = auth.CurrentUser;
            email.text = user.Email;
            tokenID.text = user.UserId;
            displayName.text = user.DisplayName;
        }
    }

    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void GoogleSignInClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticationFinished);
    }

    public void GoogleSignOutClick()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        SceneManager.LoadScene("Sign In");
    }

    private void OnGoogleAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Hata oldu!");
        }
        else if(task.IsCanceled){
            Debug.LogError("Login Canceled");
        }
        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: "+ task.Exception);
                    return;
                }

                //user = auth.CurrentUser;

                //email.text = user.Email;
                //tokenID.text = user.UserId;
                //displayName.text = user.DisplayName;

                SceneManager.LoadScene("Main");

            });
        }
    }
}
