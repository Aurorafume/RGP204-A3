using System.Collections;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel; // Reference to the ShopPanel
    private CanvasGroup shopPanelCanvasGroup;
    private Coroutine hideShopCoroutine;
    private bool isPointerOver = false;
    private float hideDelay = 1f; // 1 second delay

    void Start()
    {
        // Get the CanvasGroup component from the shop panel
        shopPanelCanvasGroup = shopPanel.GetComponent<CanvasGroup>();
        if (shopPanelCanvasGroup == null)
        {
            // CanvasGroup component missing from ShopPanel
            return;
        }

        // Initially hide the shop panel
        shopPanel.SetActive(false);
    }

    void Update()
    {
        // Check if an item is being dragged
        if (DragHandler.itemBeingDragged != null)
        {
            BoxCollider2D shopPanelCollider = shopPanel.GetComponent<BoxCollider2D>();
            if (shopPanelCollider != null)
            {
                Vector3 itemPosition = DragHandler.itemBeingDragged.transform.position;
                Vector3 itemScreenPosition = Camera.main.WorldToScreenPoint(itemPosition);
                Vector2 itemWorldPosition = Camera.main.ScreenToWorldPoint(itemScreenPosition);

                // Check if the dragged item is over the shop panel
                isPointerOver = shopPanelCollider.OverlapPoint(itemWorldPosition);
            }
        }

        // Start or stop the coroutine based on whether the item is over the shop panel
        if (DragHandler.itemBeingDragged != null && isPointerOver && hideShopCoroutine == null)
        {
            hideShopCoroutine = StartCoroutine(HideShopPanelWithDelay());
        }
        else if ((DragHandler.itemBeingDragged == null || !isPointerOver) && hideShopCoroutine != null)
        {
            StopCoroutine(hideShopCoroutine);
            hideShopCoroutine = null;
            ShowShopPanel();
        }
    }

    // Toggle the visibility of the shop panel
    public void ToggleShop()
    {
        if (DragHandler.itemBeingDragged == null)
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }

    // Handle when the pointer exits the shop panel area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == DragHandler.itemBeingDragged)
        {
            isPointerOver = false;
        }
    }

    // Coroutine to hide the shop panel after a delay
    private IEnumerator HideShopPanelWithDelay()
    {
        yield return new WaitForSeconds(hideDelay); // Wait for the delay

        if (isPointerOver && DragHandler.itemBeingDragged != null)
        {
            HideShopPanel();
        }
        hideShopCoroutine = null; // Reset the coroutine reference after completion
    }

    // Hide the shop panel
    private void HideShopPanel()
    {
        shopPanel.SetActive(false);
        if (shopPanelCanvasGroup != null)
        {
            shopPanelCanvasGroup.alpha = 0; // Make the panel invisible
        }
    }

    // Show the shop panel
    private void ShowShopPanel()
    {
        shopPanel.SetActive(true);
        if (shopPanelCanvasGroup != null)
        {
            shopPanelCanvasGroup.alpha = 1; // Make the panel visible
        }
    }
}
