using Inventory;
using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [SerializeField] private CharacterInventory inventory;
    private InventorySO inventoryData;

    private void Awake()
    {
        if (inventory == null) Debug.LogWarning("Inventory missing");
        inventory = GetComponent<CharacterInventory>();
        inventoryData = inventory.inventoryData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inventory == null)
        {
            Debug.LogWarning("Inventory missing");
            return;
        }
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0)
            {
                item.DestroyItem();
            }
            else
            {
                item.Quantity = reminder;
            }
        }
    }
}
