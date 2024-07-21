using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Load the instructions scene
    public void PlayGame()
    {
        SceneManager.LoadScene("Instructions");
    }

    // Exit the application
    public void ExitGame()
    {
        Application.Quit();
    }
}
