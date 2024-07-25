using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 originalPosition;
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f; // Change its transparency 
        canvasGroup.blocksRaycasts = false;

        originalPosition = rectTransform.anchoredPosition; // Capture original position
        originalScale = rectTransform.localScale; // Capture original scale
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        //play sound
        AudioManager.instance.PlaySoundEffect("ObjectDrop");


        // Check if the item is dropped on a valid slot
        if (!ValidDrop(eventData))
        {
            ResetPosition();
        }
        else
        {
            // Ensure the scale is reset to the original
            rectTransform.localScale = originalScale;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localScale = originalScale; // Reset the scale to the original
    }

    private bool ValidDrop(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<ItemSlot>() != null)
            {
                return true;
            }
        }
        return false;
    }
}
