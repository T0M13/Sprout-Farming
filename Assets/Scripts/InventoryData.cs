using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryData : MonoBehaviour
    {
        public InventorySO inventoryData;

        private void Awake()
        {
            if(inventoryData == null)
            {
                Debug.Log("Inventory not added or found");
            }
        }
    }
}