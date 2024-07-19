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
        shopPanelCanvasGroup = shopPanel.GetComponent<CanvasGroup>();
        if (shopPanelCanvasGroup == null)
        {
            Debug.LogError("CanvasGroup component missing from ShopPanel GameObject. Please add it.");
            return;
        }

        shopPanel.SetActive(false);
    }

    void Update()
    {
        if (DragHandler.itemBeingDragged != null)
        {
            BoxCollider2D shopPanelCollider = shopPanel.GetComponent<BoxCollider2D>();
            if (shopPanelCollider != null)
            {
                Vector3 itemPosition = DragHandler.itemBeingDragged.transform.position;
                Vector3 itemScreenPosition = Camera.main.WorldToScreenPoint(itemPosition);
                Vector2 itemWorldPosition = Camera.main.ScreenToWorldPoint(itemScreenPosition);

                if (shopPanelCollider.OverlapPoint(itemWorldPosition))
                {
                    isPointerOver = true;
                    Debug.Log("Pointer is over the shop panel");
                }
                else
                {
                    isPointerOver = false;
                    Debug.Log("Pointer is not over the shop panel");
                }
            }
            else
            {
                Debug.LogError("Shop panel collider not found");
            }
        }

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

    public void ToggleShop()
    {
        if (DragHandler.itemBeingDragged == null)
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log("Pointer entered a collider: " + other.gameObject.name);
    //     if (DragHandler.itemBeingDragged != null && other.gameObject == DragHandler.itemBeingDragged)
    //     {
    //         Debug.Log("Pointer entered shop panel area with dragged item: " + other.gameObject.name);
    //         isPointerOver = true;
    //     }
    // }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == DragHandler.itemBeingDragged)
        {
            Debug.Log("Pointer exited shop panel area with dragged item: " + other.gameObject.name);
            isPointerOver = false;
        }
    }

    private IEnumerator HideShopPanelWithDelay()
    {
        Debug.Log("Starting coroutine to hide shop panel after delay");
        yield return new WaitForSeconds(hideDelay); // Wait for the delay

        if (isPointerOver && DragHandler.itemBeingDragged != null)
        {
            Debug.Log("Hiding shop panel");
            HideShopPanel();
        }
        hideShopCoroutine = null; // Reset the coroutine reference after completion
    }

    private void HideShopPanel()
    {
        shopPanel.SetActive(false);
        if (shopPanelCanvasGroup != null)
        {
            shopPanelCanvasGroup.alpha = 0; // Make the panel invisible
        }
    }

    private void ShowShopPanel()
    {
        shopPanel.SetActive(true);
        if (shopPanelCanvasGroup != null)
        {
            shopPanelCanvasGroup.alpha = 1; // Make the panel visible
        }
    }
}
