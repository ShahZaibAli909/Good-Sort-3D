using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot3D : MonoBehaviour, IDropHandler
{
    private GameObject currentItem;
    private BoxCollider slotCollider;

    public Vector3 originalColliderSize;
    public Vector3 adjustedColliderSize;

    public Vector3 originalColliderCenter;
    public Vector3 adjustedColliderCenter;

    public bool hasItem;

    private void Awake()
    {
        slotCollider = GetComponent<BoxCollider>();
        ResetCollider(); // Initialize collider size
    }

    private void Update()
    {
        // Check if there's a child item and update collider size
        if (transform.childCount > 0)
        {
            GameObject childItem = transform.GetChild(0).gameObject;
            if (childItem != currentItem)
            {
                // If the current item is different, update the current item and collider size
                SetCurrentItem(childItem);
            }
        }
        else if (currentItem != null)
        {
            // If there's no child but current item is set, clear it
            ClearCurrentItem();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DragDrop3D dragDrop = eventData.pointerDrag.GetComponent<DragDrop3D>();
            Item item = eventData.pointerDrag.GetComponent<Item>();

            if (item != null && dragDrop != null && CanPlaceItem(eventData.pointerDrag))
            {
                // Set the item as a child of the slot and position it correctly
                Transform itemTransform = eventData.pointerDrag.transform;
                itemTransform.SetParent(transform);
                itemTransform.localPosition = Vector3.zero;

                // Update the slot's item type and adjust collider size
                SetCurrentItem(eventData.pointerDrag);

                // Inform the ShelfManager to check the slots
                ShelfManager3D shelfManager = GetComponentInParent<ShelfManager3D>();
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

    public bool CanPlaceItem(GameObject item)
    {
        // Check if the slot is empty or if the item is already the current item
        return !hasItem || (hasItem && item == currentItem);
    }

    public GameObject GetCurrentItem()
    {
        return currentItem;
    }

    public void SetCurrentItem(GameObject item)
    {
        currentItem = item;
        hasItem = true;
        AdjustColliderSize(adjustedColliderSize, adjustedColliderCenter);
    }

    public void ClearCurrentItem()
    {
        currentItem = null;
        hasItem = false;
        ResetCollider();
    }

    private void AdjustColliderSize(Vector3 size, Vector3 center)
    {
        slotCollider.size = size;
        slotCollider.center = center;
    }

    public void ResetCollider()
    {
        AdjustColliderSize(originalColliderSize, originalColliderCenter);
    }

    public void HandleItemDestruction(GameObject item)
    {
        if (currentItem == item)
        {
            ClearCurrentItem(); // Clear the slot and reset the collider
        }
    }
}
