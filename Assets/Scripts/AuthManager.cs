using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class AuthManager : MonoBehaviour
{
    [Header("UI Reference")]
    public UILogin ui; // σύρε το LoginCanvas εδώ

    [Header("Next Scene")]
    public string nextScene = "StartScene"; // ✅ StartScene, όχι MainScene

    private async void Awake()
    {
        await InitializeUnityServices();
    }

    private async Task InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Unity Services initialization failed: " + e.Message);
            ui.OnError("Initialization failed!");
        }
    }

    public async void RegisterUser(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            OnLoginSuccess();
        }
        catch (System.Exception e)
        {
            ui.OnError("Register failed: " + e.Message);
        }
    }

    public async void LoginUser(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            OnLoginSuccess();
        }
        catch (System.Exception e)
        {
            ui.OnError("Login failed: " + e.Message);
        }
    }

    private void OnLoginSuccess()
    {
        ui.OnLoginSuccess();                   // ενημερώνει το UI
        SceneManager.LoadScene(nextScene);     // ✅ Φορτώνει StartScene
    }
}
