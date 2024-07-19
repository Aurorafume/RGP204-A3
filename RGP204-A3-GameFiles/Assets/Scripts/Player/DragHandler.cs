using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefab; // Reference to the prefab
    public static GameObject itemBeingDragged;
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
            Debug.Log("Dragging started: " + itemBeingDragged.name);

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
                //Debug.Log("Dragging item to position: " + anchoredPosition);
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
            Debug.Log("Dragging ended: " + itemBeingDragged.name);
        }

        itemBeingDragged = null;
    }

    private List<RaycastResult> GetRaycastResults()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results;
    }
}
