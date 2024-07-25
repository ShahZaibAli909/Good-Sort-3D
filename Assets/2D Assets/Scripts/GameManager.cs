using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<ShelfManager> shelfManagers = new List<ShelfManager>();
    private HashSet<string> allItemTypes = new HashSet<string>();
    private HashSet<string> matchedItemTypes = new HashSet<string>();
    [SerializeField] private GameObject WinText;
    private void Start()
    {
        WinText.SetActive(false);
        // Find all ShelfManager instances in the scene
        ShelfManager[] shelves = FindObjectsOfType<ShelfManager>();
        foreach (ShelfManager shelf in shelves)
        {
            shelfManagers.Add(shelf);
        }

        // Populate allItemTypes with all the unique item types present in the game
        InitializeAllItemTypes();
    }

    private void InitializeAllItemTypes()
    {
        // Assuming we have a reference to all items in the game (e.g., from a GameManager or similar)
        Item[] allItems = FindObjectsOfType<Item>();
        foreach (Item item in allItems)
        {
            allItemTypes.Add(item.itemType);
        }
    }

    public void CheckWinCondition()
    {
        matchedItemTypes.Clear();

        // Check each ShelfManager for matched item types
        foreach (ShelfManager shelfManager in shelfManagers)
        {
            foreach (string itemType in shelfManager.GetMatchedItemTypes())
            {
                matchedItemTypes.Add(itemType);
            }
        }

        // Check if all item types have been matched
        if (AllTypesMatchedAndDestroyed())
        {
            //play sound
            AudioManager.instance.Stop();
            AudioManager.instance.PlaySoundEffect("Win");

            WinText.SetActive(true);
            Time.timeScale = 0f;

        }
    }

    private bool AllTypesMatchedAndDestroyed()
    {
        // Check if matchedItemTypes contains all the item types in the game
        return matchedItemTypes.SetEquals(allItemTypes);
    }
}
