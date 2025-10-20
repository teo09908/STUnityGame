using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel; // drag & drop MainMenuPanel here
    public UnityEvent OnPlayPressed;  // event για να ξεκινήσει το παιχνίδι (π.χ. spawn rooms)

    void Start()
    {
        // Κρύβουμε το παιχνίδι μέχρι να πατηθεί Play
        Time.timeScale = 0f; // παγώνει το παιχνίδι
    }

    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f; // ξεπαγώνει το παιχνίδι

        if (OnPlayPressed != null)
            OnPlayPressed.Invoke(); // ξεκινάει το board / event system
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
}
