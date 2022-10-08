using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class ChestInventoryController : InventoryController
    {
        public CanvasInventory canvas;
        public RectTransform chestInventoryPanel;

        public override void Awake()
        {
            canvas = FindObjectOfType<CanvasInventory>();
            base.Awake();
        }

        public override void Start()
        {
            RectTransform chestInventoryPanelClone = Instantiate(chestInventoryPanel, chestInventoryPanel.transform.position, chestInventoryPanel.transform.rotation);
            UIInventoryChestPage uiInventory = chestInventoryPanelClone.GetComponent<UIInventoryChestPage>();
            uiInventory.mouseFollower = canvas.mouseFollower;
            uiInventory.CustomAwake();
            chestInventoryPanelClone.SetParent(canvas.chestInventories, false);
            chestInventoryPanelClone.transform.localScale = Vector3.one;
            inventoryUI = uiInventory;
            base.Start();
        }

    }
}