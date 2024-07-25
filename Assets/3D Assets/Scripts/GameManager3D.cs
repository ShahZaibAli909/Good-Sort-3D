using System.Collections.Generic;
using UnityEngine;

public class GameManager3D : MonoBehaviour
{
    public List<ShelfManager3D> shelfManagers = new List<ShelfManager3D>();
    private HashSet<string> allItemTypes = new HashSet<string>();
    private HashSet<string> matchedItemTypes = new HashSet<string>();
    [SerializeField] private GameObject WinText;

    private void Start()
    {
        WinText.SetActive(false);

        // Clear the list to avoid duplication
        shelfManagers.Clear();

        // Find all ShelfManager3D instances in the scene
        ShelfManager3D[] shelves = FindObjectsOfType<ShelfManager3D>();
        foreach (ShelfManager3D shelf in shelves)
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

        // Check each ShelfManager3D for matched item types
        foreach (ShelfManager3D shelfManager in shelfManagers)
        {
            foreach (string itemType in shelfManager.GetMatchedItemTypes())
            {
                matchedItemTypes.Add(itemType);
            }
        }

        // Check if all item types have been matched
        if (AllTypesMatchedAndDestroyed())
        {
            // Play sound
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
