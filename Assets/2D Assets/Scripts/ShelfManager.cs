using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    public List<ItemSlot> slots = new List<ItemSlot>();
    public int requiredItemsToMatch = 3;
    private HashSet<string> matchedItemTypes = new HashSet<string>();

    private void Start()
    {
        if (slots.Count == 0)
        {
            foreach (Transform child in transform)
            {
                ItemSlot slot = child.GetComponent<ItemSlot>();
                if (slot != null)
                {
                    slots.Add(slot);
                }
            }
        }
    }//start method

    public void CheckSlots()
    {
        Dictionary<string, int> itemCount = new Dictionary<string, int>();

        foreach (ItemSlot slot in slots)
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

                    if (itemCount[itemType] >= requiredItemsToMatch)
                    {
                        DestroyItems(itemType);
                        //play sound
                        AudioManager.instance.PlaySoundEffect("ItemsMatched");
                        matchedItemTypes.Add(itemType);
                        // Notify the GameManager to check the win condition
                        FindObjectOfType<GameManager>().CheckWinCondition();
                    }
                }
            }
        }
    }//check slot method

    private void DestroyItems(string itemType)
    {
        foreach (ItemSlot slot in slots)
        {
            GameObject currentItem = slot.GetCurrentItem();
            if (currentItem != null && currentItem.GetComponent<Item>().itemType == itemType)
            {
                Destroy(currentItem);
                
            }
        }
    }//destroy items method

    public HashSet<string> GetMatchedItemTypes()
    {
        return matchedItemTypes;
    }
}
