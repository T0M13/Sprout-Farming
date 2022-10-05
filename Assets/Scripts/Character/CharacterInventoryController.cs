using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class CharacterInventoryController : MonoBehaviour
    {

        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private CharacterInventory inventory;
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        [Header("Input Settings")]
        [SerializeField] private InputActionReference _playerInventory;

        #region Input
        private void OnEnable()
        {
            _playerInventory.action.Enable();
        }

        private void OnDisable()
        {
            _playerInventory.action.Disable();
        }
        #endregion

        private void Awake()
        {
            if (inventoryUI == null)
            {
                Debug.Log("Inventory UI has not been added or found");
            }

            if (inventory == null) Debug.Log("Inventory missing");
            inventory = GetComponent<CharacterInventory>();
            inventoryData = inventory.inventoryData;

        }

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
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

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            if (inventoryUI == null) return;
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
            inventoryUI.OnEndDrag += HandleEndDrag;
        }

        private void HandleEndDrag(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            DropItem(itemIndex, inventoryItem.quantity);
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            //RightClick

        }

        public void RefreshQuantity(int itemIndex)
        {
            inventoryUI.RefreshAction();
            HandleDescriptionRequest(itemIndex);
        }

        public void PerformAction(int itemIndex)
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

        private void DropItem(int itemIndex, int quantity)
        {

            CreateItem(itemIndex, quantity);
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();

        }

        private void CreateItem(int itemIndex, int quantity)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            Vector3 itemSpawnOffset = new Vector3(5, 0, 0);
            GameObject itemClone = Instantiate(inventoryData.customItemPrefab, transform.position + itemSpawnOffset, Quaternion.identity);
            Item itemScript = itemClone.GetComponent<Item>();
            itemScript.SetItem(inventoryItem.item, quantity);
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);

        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
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

        private string PrepareDescription(InventoryItem inventoryItem)
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

        private void Update()
        {
            if (inventoryUI == null) return;
            if (_playerInventory.action.triggered)
                ToggleInventory();
        }

        private void ToggleInventory()
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