using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {

        public UIInventoryPage inventoryUI;
        public InventoryData inventory;
        public InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();


        /// <summary>
        /// Get the Inventory from Character
        /// </summary>
        public virtual void Awake()
        {
            if (inventoryUI == null)
            {
                Debug.Log("Inventory UI has not been added or found");
            }

            if (inventory == null) Debug.Log("Inventory missing");
            inventory = GetComponent<InventoryData>();
            inventoryData = inventory.inventoryData;

        }

        /// <summary>
        /// Prepares the Inventory UI and Data
        /// </summary>
        public virtual void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        public virtual void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        public virtual void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        public virtual void PrepareUI()
        {
            if (inventoryUI == null) return;
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
            inventoryUI.OnEndDrag += HandleEndDrag;
        }

        public virtual void HandleEndDrag(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            DropItem(itemIndex, inventoryItem.quantity);
        }

        public virtual void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            //RightClick

        }

        public virtual void RefreshQuantity(int itemIndex)
        {
            inventoryUI.RefreshAction();
            HandleDescriptionRequest(itemIndex);
        }

        public virtual void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerfomAction(gameObject, inventoryItem.itemState);
                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.ResetSelection();
            }

            RefreshQuantity(itemIndex);

        }

        public virtual void DropItem(int itemIndex, int quantity)
        {

            CreateItem(itemIndex, quantity);
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();

        }

        public virtual void CreateItem(int itemIndex, int quantity)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            Vector3 itemSpawnOffset = DropOffset();

            //check if its on water or other unreachable places

            GameObject itemClone = Instantiate(inventoryData.customItemPrefab, transform.position + itemSpawnOffset, Quaternion.identity);
            itemClone.name = $"{inventoryItem.item.name}_Item";
            Item itemScript = itemClone.GetComponent<Item>();
            itemScript.SetItem(inventoryItem.item, quantity);
        }

        public virtual Vector3 DropOffset()
        {

            float randomPositionX = Random.Range(0, 2) * 2 - 1;
            float randomPositionY = Random.Range(0, 2) * 2 - 1;
            return new Vector3(randomPositionX, randomPositionY, 0);
        }

        public virtual void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);

        }

        public virtual void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        public virtual void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, description);




            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowHorizontalItemAction(itemIndex);
                inventoryUI.AddActionHorizontal(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddActionHorizontal("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }

        }

        public virtual string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName}" + $": {inventoryItem.itemState[i].value} / " + $"{inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }


        public virtual void ToggleInventory()
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                }
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }
}