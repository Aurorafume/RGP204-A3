using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum ItemType { WateringCan, Shovel, PlantSeeds, VineSeeds, Pot }
    public ItemType itemType;
    public GameObject prefab; // Reference to the prefab
    public static GameObject itemBeingDragged;
    public static ItemType itemTypeBeingDragged;
    private CanvasGroup canvasGroup;
    private GameObject objectsContainer;
    private RectTransform canvasRectTransform;

    private EconomyManager economyManager;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();

        objectsContainer = GameObject.Find("-Objects-");
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRectTransform = parentCanvas.GetComponent<RectTransform>();
        }

        // Find the EconomyManager in the scene
        economyManager = GameObject.FindObjectOfType<EconomyManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prefab != null && economyManager != null)
        {
            // Check if the item is PlantSeeds or VineSeeds and if the player can afford it
            if ((itemType == ItemType.PlantSeeds || itemType == ItemType.VineSeeds) && economyManager.CanAfford(10))
            {
                economyManager.SubtractMoney(10); // Deduct $10 when seeds are purchased

                if (canvasRectTransform == null || objectsContainer == null)
                {
                    return;
                }

                itemBeingDragged = Instantiate(prefab, objectsContainer.transform);
                itemBeingDragged.tag = "Seed"; // Set the tag to "Seed"
                itemTypeBeingDragged = itemType;

                RectTransform itemRectTransform = itemBeingDragged.GetComponent<RectTransform>();
                if (itemRectTransform != null)
                {
                    Vector2 anchoredPosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, eventData.pressEventCamera, out anchoredPosition);
                    itemRectTransform.anchoredPosition = anchoredPosition;

                    itemRectTransform.localPosition = new Vector3(itemRectTransform.localPosition.x, itemRectTransform.localPosition.y, 0);

                    itemRectTransform.localScale = Vector3.one;
                    itemRectTransform.sizeDelta = new Vector2(100, 100); // Adjust size as necessary
                }

                canvasGroup.blocksRaycasts = false;
            }
            else if (itemType != ItemType.PlantSeeds && itemType != ItemType.VineSeeds)
            {
                if (canvasRectTransform == null || objectsContainer == null)
                {
                    return;
                }

                itemBeingDragged = Instantiate(prefab, objectsContainer.transform);
                itemTypeBeingDragged = itemType;

                RectTransform itemRectTransform = itemBeingDragged.GetComponent<RectTransform>();
                if (itemRectTransform != null)
                {
                    Vector2 anchoredPosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, eventData.pressEventCamera, out anchoredPosition);
                    itemRectTransform.anchoredPosition = anchoredPosition;

                    itemRectTransform.localPosition = new Vector3(itemRectTransform.localPosition.x, itemRectTransform.localPosition.y, 0);

                    itemRectTransform.localScale = Vector3.one;
                    itemRectTransform.sizeDelta = new Vector2(100, 100); // Adjust size as necessary
                }

                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemBeingDragged != null)
        {
            RectTransform itemRectTransform = itemBeingDragged.GetComponent<RectTransform>();
            if (itemRectTransform != null)
            {
                Vector2 anchoredPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, eventData.pressEventCamera, out anchoredPosition);
                itemRectTransform.anchoredPosition = anchoredPosition;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        if (itemBeingDragged != null)
        {
            // Handle seed placement in pots
            if (itemTypeBeingDragged == ItemType.PlantSeeds || itemTypeBeingDragged == ItemType.VineSeeds)
            {
                Vector2 itemPosition = itemBeingDragged.transform.position;
                float detectionRadius = 1.0f; // Adjust this radius as necessary
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(itemPosition, detectionRadius);

                bool seedPlaced = false;

                foreach (Collider2D collider in hitColliders)
                {
                    Plant plant = itemBeingDragged.GetComponent<Plant>();

                    if (itemTypeBeingDragged == ItemType.PlantSeeds && collider.CompareTag("Pot"))
                    {
                        if (plant != null)
                        {
                            plant.seedType = SeedType.Normal;
                            plant.isPlanted = true;
                            plant.SetImageForCurrentStage();
                            SnapToPot(itemBeingDragged.transform, collider);
                            seedPlaced = true;
                            break;
                        }
                    }
                    else if (itemTypeBeingDragged == ItemType.VineSeeds && collider.CompareTag("VinePot"))
                    {
                        if (plant != null)
                        {
                            plant.seedType = SeedType.Vine;
                            plant.isPlanted = true;
                            plant.SetImageForCurrentStage();
                            SnapToPot(itemBeingDragged.transform, collider);
                            seedPlaced = true;
                            break;
                        }
                    }
                }

                if (!seedPlaced)
                {
                    Destroy(itemBeingDragged);
                }
            }
            else if (itemTypeBeingDragged == ItemType.WateringCan)
            {
                Vector2 itemPosition = itemBeingDragged.transform.position;
                float detectionRadius = 1.0f; // Adjust this radius as necessary
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(itemPosition, detectionRadius);

                foreach (Collider2D collider in hitColliders)
                {
                    Plant plant = collider.GetComponent<Plant>();

                    if (plant != null)
                    {
                        plant.WaterPlant();
                        // Destroy the watering can after use
                        Destroy(itemBeingDragged);
                    }

                    Destroy(itemBeingDragged);
                }
            }
            else if (itemTypeBeingDragged == ItemType.Shovel)
            {
                Vector2 itemPosition = itemBeingDragged.transform.position;
                float detectionRadius = 1.0f; // Adjust this radius as necessary
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(itemPosition, detectionRadius);

                foreach (Collider2D collider in hitColliders)
                {
                    Plant plant = collider.GetComponent<Plant>();
                    
                    // If the plant is mature, sell it
                    if (plant != null && plant.currentStage == GrowthStage.Mature)
                    {
                        plant.SellPlant();
                        Destroy(itemBeingDragged);
                    }
                    else if (plant != null)
                    {
                        plant.RemovePlant();
                        Destroy(itemBeingDragged);
                    }

                    Destroy(itemBeingDragged);
                }
            }
            else if (itemTypeBeingDragged == ItemType.Pot)
            {
                Vector2 itemPosition = itemBeingDragged.transform.position;
                float detectionRadius = 0.5f; // Adjust this radius to be smaller and more precise
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(itemPosition, detectionRadius);

                bool potPlaced = false;

                foreach (Collider2D collider in hitColliders)
                {
                    HiddenPot hiddenPot = collider.GetComponent<HiddenPot>();

                    if (hiddenPot != null)
                    {
                        // Show the hidden pot
                        hiddenPot.ShowPot();

                        // Destroy the dragged pot (from the shop) since it is now replaced by the scene pot
                        Destroy(itemBeingDragged);

                        potPlaced = true;
                        break;
                    }
                }

                if (!potPlaced)
                {
                    // If no valid hidden pot was found, return the dragged pot back to its original position or destroy it.
                    Destroy(itemBeingDragged); // Or reset its position if you want to allow re-dragging
                }
            }
            else
            {
                Destroy(itemBeingDragged);
            }

            itemBeingDragged = null;
        }
    }

    // Snap the seed to the pot's position
    private void SnapToPot(Transform seedTransform, Collider2D potCollider)
    {
        float offsetY = potCollider.bounds.size.y / 2; // Calculate the offset to position at the top
        seedTransform.position = new Vector3(potCollider.bounds.center.x, potCollider.bounds.max.y + offsetY, seedTransform.position.z);
        seedTransform.SetParent(GameObject.Find("-Objects-").transform); // Set the plant under the "-Objects-" gameobject
    }

}
