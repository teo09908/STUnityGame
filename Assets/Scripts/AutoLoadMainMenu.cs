using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLoadMainMenu : MonoBehaviour
{
    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

