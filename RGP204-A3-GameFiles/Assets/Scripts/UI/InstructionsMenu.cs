using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
