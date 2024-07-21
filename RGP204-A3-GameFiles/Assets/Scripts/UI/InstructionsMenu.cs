using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    // Load the main game scene
    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    // Return to the main menu
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Exit the application
    public void ExitGame()
    {
        Application.Quit();
    }
}
