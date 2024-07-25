using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    public string itemType;

    public string ItemType
    {
        get { return itemType; }
        set { itemType = value; }
    }

    public bool CanBeDragged { get; set; } = true; // Manage whether the item can be dragged
}
