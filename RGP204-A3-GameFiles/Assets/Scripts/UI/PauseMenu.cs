using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private void Start()
    {
        pauseMenuUI.SetActive(false); // Ensure the pause menu is hidden at start
    }

    void Update()
    {
        // Toggle pause menu with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resume the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // Pause the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // Load the main menu
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // Exit the application
    public void ExitGame()
    {
        Application.Quit();
    }

    // These methods can be called by the buttons
    public void OnResumeButton()
    {
        Resume();
    }

    public void OnLoadMenuButton()
    {
        LoadMenu();
    }

    public void OnExitGameButton()
    {
        ExitGame();
    }
}
