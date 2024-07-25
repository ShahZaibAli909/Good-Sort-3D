using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ShelfManager3D : MonoBehaviour
{
    public List<ItemSlot3D> slots = new List<ItemSlot3D>();
    public int requiredItemsToMatch = 3;
    private HashSet<string> matchedItemTypes = new HashSet<string>();

    private void Start()
    {
        if (slots.Count == 0)
        {
            foreach (Transform child in transform)
            {
                ItemSlot3D slot = child.GetComponent<ItemSlot3D>();
                if (slot != null)
                {
                    slots.Add(slot);
                }
            }
        }
    }

    public void CheckSlots()
    {
        // First check if all slots have a child item
        bool allSlotsFilled = true;
        foreach (ItemSlot3D slot in slots)
        {
            if (slot.GetCurrentItem() == null)
            {
                allSlotsFilled = false;
                break;
            }
        }

        if (!allSlotsFilled)
        {
            Debug.Log("Not all slots are filled. Skipping check.");
            return;
        }

        // Clear item counts for each type
        Dictionary<string, int> itemCount = new Dictionary<string, int>();

        foreach (ItemSlot3D slot in slots)
        {
            GameObject currentItem = slot.GetCurrentItem();
            if (currentItem != null)
            {
                Item item = currentItem.GetComponent<Item>();
                if (item != null)
                {
                    string itemType = item.itemType;
                    if (itemCount.ContainsKey(itemType))
                    {
                        itemCount[itemType]++;
                    }
                    else
                    {
                        itemCount[itemType] = 1;
                    }
                }
            }
        }

        // Check and destroy items if their count meets the required number
        List<string> itemsToDestroy = new List<string>();

        foreach (KeyValuePair<string, int> entry in itemCount)
        {
            string itemType = entry.Key;
            int count = entry.Value;
            Debug.Log($"ItemType: {itemType}, Count: {count}");

            if (count >= requiredItemsToMatch && !matchedItemTypes.Contains(itemType))
            {
                Debug.Log($"Match found for ItemType: {itemType}");
                itemsToDestroy.Add(itemType);
                matchedItemTypes.Add(itemType);
            }
        }

        foreach (string itemType in itemsToDestroy)
        {
            StartCoroutine(DestroyItemsWithDelay(itemType));
        }

        // Notify the GameManager to check the win condition
        FindObjectOfType<GameManager3D>().CheckWinCondition();
        // Play sound
        AudioManager.instance.PlaySoundEffect("ItemsMatched");
    }

    private IEnumerator DestroyItemsWithDelay(string itemType)
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds

        foreach (ItemSlot3D slot in slots)
        {
            GameObject currentItem = slot.GetCurrentItem();
            if (currentItem != null && currentItem.GetComponent<Item>().itemType == itemType)
            {
                slot.ResetCollider(); // Reset the collider to its original size and center before destroying
                slot.HandleItemDestruction(currentItem); // Clear the slot
                Destroy(currentItem); // Destroy the item
            }
        }
    }

    public HashSet<string> GetMatchedItemTypes()
    {
        return matchedItemTypes;
    }
}
