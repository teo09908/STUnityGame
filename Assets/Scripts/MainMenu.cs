using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel; // drag & drop MainMenuPanel here
    public UnityEvent OnPlayPressed;  // event ��� �� ��������� �� �������� (�.�. spawn rooms)

    void Start()
    {
        // �������� �� �������� ����� �� ������� Play
        Time.timeScale = 0f; // ������� �� ��������
    }

    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f; // ��������� �� ��������

        if (OnPlayPressed != null)
            OnPlayPressed.Invoke(); // �������� �� board / event system
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
}
