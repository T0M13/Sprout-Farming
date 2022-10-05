using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryEquipSlot : MonoBehaviour
    {
    
        public UIInventoryItem inventoryItem;
        [SerializeField] private EquippableItemSO equippableItemSO;

        public void SetEquippedItem(EquippableItemSO equippableItemSO)
        {
            this.equippableItemSO = equippableItemSO;
            inventoryItem.SetData(equippableItemSO.ItemImage, 1);
        }

    }
}