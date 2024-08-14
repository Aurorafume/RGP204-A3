using UnityEngine;
using UnityEngine.UI;

public class HiddenPot : MonoBehaviour
{
    private Image imageComponent;
    private Collider2D collider2D;
    private Sprite originalSprite;  // To store the original image sprite
    private Color originalColor;    // To store the original image color

    void Start()
    {
        // Get the Image and Collider2D components
        imageComponent = GetComponent<Image>();
        collider2D = GetComponent<Collider2D>();

        // Store the original sprite and color
        if (imageComponent != null)
        {
            originalSprite = imageComponent.sprite;
            originalColor = imageComponent.color;
        }

        // Hide the pot initially by replacing the image with a transparent white box
        HidePot();
    }

    // Method to hide the pot and replace it with a transparent white box
    public void HidePot()
    {
        if (imageComponent != null)
        {
            // Replace the current sprite with a transparent white box
            imageComponent.sprite = null;  // Remove the sprite
            imageComponent.color = new Color(1f, 1f, 1f, 0.5f);  // Set color to semi-transparent white
        }

        if (collider2D != null)
        {
            collider2D.enabled = true;  // Enable the collider so it can detect the "Pot" tag
        }
    }

    // Method to show the pot and revert to the original image
    public void ShowPot()
    {
        if (imageComponent != null)
        {
            // Restore the original sprite and color
            imageComponent.sprite = originalSprite;
            imageComponent.color = originalColor;
        }

        if (collider2D != null)
        {
            collider2D.enabled = true;  // Ensure the collider is enabled for interactions
        }
    }

    // This method is called when another collider enters this collider's trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to a GameObject with the tag "Pot"
        if (other.CompareTag("Pot"))
        {
            // Unhide this pot
            ShowPot();

            // Optionally, you can destroy the object that collided with the hidden pot
            Destroy(other.gameObject); // Only do this if you want to remove the dropped pot from the shop
        }
    }
}
