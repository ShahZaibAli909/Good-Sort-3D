using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public Image imageTransparency;
    private ShelfManager shelfManager;

    void Start()
    {
        imageTransparency = GetComponent<Image>();
        shelfManager = GetComponentInParent<ShelfManager>();

        if (shelfManager == null)
        {
            Debug.LogError("ShelfManager not found in parent.");
        }
    }

    void OnEnable()
    {
        if (imageTransparency != null)
        {
            var imageColor = imageTransparency.color;
            imageColor.a = 0f;
            imageTransparency.color = imageColor;
        }
    }

    void OnDisable()
    {
        if (imageTransparency != null)
        {
            var imageColor = imageTransparency.color;
            imageColor.a = 1f;
            imageTransparency.color = imageColor;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();

            if (draggedRectTransform != null && transform.childCount == 0)
            {
                // Set the item as a child of the slot
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.transform.SetAsLastSibling();

                // Set anchor and pivot to bottom center so item will be in bottom center

                draggedRectTransform.anchorMin = new Vector2(0.5f, 0);
                draggedRectTransform.anchorMax = new Vector2(0.5f, 0);
                draggedRectTransform.pivot = new Vector2(0.5f, 0);

                // Set anchored position to the bottom center of the slot

                draggedRectTransform.anchoredPosition = new Vector2(0, 0);

                if (shelfManager != null)
                {
                    shelfManager.CheckSlots();
                }
                else
                {
                    Debug.LogError("ShelfManager is null in OnDrop");
                }
            }
            else
            {
                if (dragDrop != null)
                {
                    dragDrop.ResetPosition();
                }
            }
        }
    }

    public GameObject GetCurrentItem()
    {
        return transform.childCount > 0 ? transform.GetChild(0).gameObject : null;
    }
}
