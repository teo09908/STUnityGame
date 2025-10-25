using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel; // assign MainMenuPanel here

    void Start()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        // allow UI to work
        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        // unfreeze time for gameplay
        Time.timeScale = 1f;

        // load the Main scene
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
}

