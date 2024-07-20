using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();

        objectsContainer = GameObject.Find("-Objects-");
        if (objectsContainer == null)
        {
            Debug.LogError("Objects container not found! Please create an empty GameObject named '-Objects-' in the scene.");
        }

        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRectTransform = parentCanvas.GetComponent<RectTransform>();
        }

        if (canvasRectTransform == null)
        {
            Debug.LogError("Canvas RectTransform not found! Please ensure this script is attached to a UI element within a Canvas.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prefab != null)
        {
            if (canvasRectTransform == null || objectsContainer == null)
            {
                Debug.LogError("Missing references. Cannot proceed with dragging.");
                return;
            }

            itemBeingDragged = Instantiate(prefab, objectsContainer.transform);
            itemTypeBeingDragged = itemType;
            Debug.Log("Dragging started: " + itemBeingDragged.name + " of type " + itemTypeBeingDragged);

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
        else
        {
            Debug.LogError("Prefab is not assigned in the DragHandler script.");
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

            Debug.Log("Dragging item: " + itemBeingDragged.name);
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
            Debug.Log("Dragging ended: " + itemBeingDragged.name + " of type " + itemTypeBeingDragged);
        }

        // if the item is the watering can tag "Water" and comes into contact with the plant tag "Plant" then the plant will be watered
        if (itemTypeBeingDragged == ItemType.WateringCan)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Plant"))
            {
                Plant plant = hit.collider.GetComponent<Plant>();
                if (plant != null)
                {
                    plant.WaterPlant();
                    // remove the dropped item from the scene
                    Destroy(itemBeingDragged);
                }
            }
        }

        // if the item is the shovel tag "Shovel" and comes into contact with the plant tag "Plant" then the plant will be removed if it is withered or sold if it is mature
        if (itemTypeBeingDragged == ItemType.Shovel)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Plant"))
            {
                Plant plant = hit.collider.GetComponent<Plant>();
                if (plant != null)
                {
                    if (plant.currentStage == GrowthStage.Withered) // Corrected this line
                    {
                        plant.RemovePlant();
                        // remove the dropped item from the scene
                        Destroy(itemBeingDragged);
                    }
                    else if (plant.currentStage == GrowthStage.Mature) // Corrected this line
                    {
                        plant.SellPlant();
                        // remove the dropped item from the scene
                        Destroy(itemBeingDragged);
                    }
                }
            }
        }

        itemBeingDragged = null;
    }
}
