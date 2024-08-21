using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    // Load the main game scene
    public void PlayIndoor()
    {
        SceneManager.LoadScene("Main");
    }

    // Load the outdoor game scene
    public void PlayOutdoor()
    {
        SceneManager.LoadScene("Main Outdoor");
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
