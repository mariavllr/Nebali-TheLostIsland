using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    [SerializeField] public List<InventoryItem> inventory;

    public delegate void OnInventoryChangedEvent();
    public static event OnInventoryChangedEvent onInventoryChangedEvent;

    private void Awake()
    {
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }

        return null;
    }



    public void Add(InventoryItemData referenceData)
    {
        
        if(itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }

        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            itemDictionary.Add(referenceData, newItem);
        }

        if (onInventoryChangedEvent != null) onInventoryChangedEvent();
    }

    public void Remove(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();

            if(value.stackSize == 0)
            {
                inventory.Remove(value);
                itemDictionary.Remove(referenceData);
            }
        }
    }
}
