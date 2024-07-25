using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop3D : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private float dragZ = 2.11f; // Z position to use when dragging

    private Camera mainCamera;
    private Vector3 offset;
    private Vector3 originalPosition;
    private float originalZ;

    private ItemSlot3D originalSlot;
    private ItemSlot3D currentSlot;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Save the original position
        originalPosition = transform.position;
        originalZ = originalPosition.z;

        // Calculate offset
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, originalZ));
        offset = transform.position - mouseWorldPosition;

        // Increase Z position to bring the item to the foreground
        transform.position = new Vector3(transform.position.x, transform.position.y, dragZ);

        // Clear the current item from the slot it was in
        if (transform.parent != null)
        {
            originalSlot = transform.parent.GetComponent<ItemSlot3D>();
            if (originalSlot != null)
            {
                originalSlot.ClearCurrentItem();
            }
        }

        // Optionally disable CanvasGroup to avoid UI blocking
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the item position based on mouse movement
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, originalZ));
        transform.position = new Vector3(mouseWorldPosition.x + offset.x, mouseWorldPosition.y + offset.y, dragZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset Z position to original after dragging
        transform.position = new Vector3(transform.position.x, transform.position.y, originalZ);

        // Reset alpha and blocksRaycasts
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        // Check if the item is dropped on a valid slot
        if (currentSlot != null && currentSlot.CanPlaceItem(gameObject))
        {
            currentSlot.SetCurrentItem(gameObject);
            transform.SetParent(currentSlot.transform);
            transform.localPosition = Vector3.zero; // Ensure the item is positioned correctly in the slot
        }
        else
        {
            ResetPosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Optional: Implement logic for pointer down if needed
    }

    public void ResetPosition()
    {
        // Reset position to original
        transform.position = originalPosition;

        // Reassign the item back to the original slot if drop is invalid
        if (originalSlot != null)
        {
            originalSlot.SetCurrentItem(gameObject);
            transform.SetParent(originalSlot.transform);
            transform.localPosition = Vector3.zero; // Ensure the item is positioned correctly in the original slot
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slot"))
        {
            ItemSlot3D slot = other.GetComponent<ItemSlot3D>();
            if (slot != null && slot.CanPlaceItem(gameObject))
            {
                currentSlot = slot;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slot"))
        {
            ItemSlot3D slot = other.GetComponent<ItemSlot3D>();
            if (slot == currentSlot)
            {
                currentSlot = null;
            }
        }
    }
}
