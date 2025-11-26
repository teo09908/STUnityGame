using UnityEngine;
using TMPro;

public class UILogin : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI errorMessage;
    public GameObject loadingText;

    [Header("Auth Manager")]
    public AuthManager auth;

    public void OnLoginClick()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            errorMessage.text = "Email and password required!";
            return;
        }

        loadingText.SetActive(true);
        errorMessage.text = "";

        auth.LoginUser(email, password);
    }

    public void OnRegisterClick()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            errorMessage.text = "Email and password required!";
            return;
        }

        loadingText.SetActive(true);
        errorMessage.text = "";

        auth.RegisterUser(email, password);
    }

    public void OnLoginSuccess()
    {
        loadingText.SetActive(false);
        gameObject.SetActive(false); // κρύβει το UI
    }

    public void OnError(string msg)
    {
        loadingText.SetActive(false);
        errorMessage.text = msg;
    }
}
