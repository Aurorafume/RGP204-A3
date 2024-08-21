using UnityEngine;
using UnityEngine.UI;

public class Bug : MonoBehaviour
{
    private Image bugImage;

    void Awake()
    {
        bugImage = GetComponent<Image>();
        if (bugImage != null)
        {
            bugImage.enabled = false; // Disable the Image component initially
        }
    }

    // Show the bug when needed
    public void ShowBug()
    {
        if (bugImage != null)
        {
            bugImage.enabled = true; // Enable the Image component to show the bug
        }
    }

    // Hide the bug after pesticide is applied
    public void HideBug()
    {
        if (bugImage != null)
        {
            bugImage.enabled = false; // Disable the Image component to hide the bug
        }
    }
}