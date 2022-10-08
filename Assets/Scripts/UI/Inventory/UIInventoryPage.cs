using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [Header("References")]
        public UIInventoryItem itemPrefab;
        public RectTransform contentPanel;
        public UIMouseFollower mouseFollower;
        [Header("Items in List")]
        public List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging, OnEndDrag;
        public event Action<int, int> OnSwapItems;

        public int currentlyDraggedItemIndex = -1;
        public int UILayer;

        /// <summary>
        /// Hides Inventory - and disables the mouse follower - resets item description
        /// </summary>
        public virtual void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            UILayer = LayerMask.NameToLayer("UI");
        }

        /// <summary>
        /// Starts up the inventory and it initialiues the items
        /// </summary>
        /// <param name="inventorySize"></param>
        public virtual void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                uiItem.transform.localScale = Vector3.one;
                listOfUIItems.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        public virtual void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public virtual void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        public virtual void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            InvokeOnItemActionRequested(index);
        }

        public virtual void InvokeOnItemActionRequested(int index)
        {
            OnItemActionRequested?.Invoke(index);
        }

        public virtual void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            if (currentlyDraggedItemIndex == -1)
            {
                return;
            }

            if (!IsPointerOverUIElement())
            {
                InvokeOnEndDrag(currentlyDraggedItemIndex);
                ResetDraggedItem();
            }
            else
            {
                ResetDraggedItem();
            }

        }

        public virtual void InvokeOnEndDrag(int index)
        {
            OnEndDrag?.Invoke(currentlyDraggedItemIndex);
        }

        public virtual void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        public virtual void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1 || currentlyDraggedItemIndex == -1)
            {
                return;
            }
            InvokeOnSwapItems(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        public virtual void InvokeOnSwapItems(int index_1, int index_2)
        {
            OnSwapItems?.Invoke(index_1, index_2);
        }

        public virtual void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        public virtual void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            //HandleItemSelection(inventoryItemUI);
            InvokeOnStartDragging(index);
        }
        public virtual void InvokeOnStartDragging(int index)
        {
            OnStartDragging?.Invoke(index);
        }

        public virtual void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        public virtual void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            InvokeOnDescriptionRequested(index);
        }

        public virtual void InvokeOnDescriptionRequested(int index)
        {
            OnDescriptionRequested?.Invoke(index);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();

        }

        public virtual void ResetSelection()
        {
            DeselectAllItems();
        }

        public virtual void AddActionHorizontal(string actionName, Action performAction)
        {
        }

        public virtual void RefreshAction()
        {
        }

        public virtual void ShowHorizontalItemAction(int itemIndex)
        {
        }

        public virtual void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggedItem();
        }





        //Returns 'true' if we are touching or hovering on Unity UI element.
        public virtual bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }


        //Returns 'true' if we are touching or hovering on Unity UI element.
        public virtual bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == UILayer)
                    return true;
            }
            return false;
        }

        //Gets all event system raycast results of current mouse or touch position.
        public virtual List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}