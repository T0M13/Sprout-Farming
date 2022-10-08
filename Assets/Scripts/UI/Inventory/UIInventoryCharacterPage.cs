using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UIInventoryCharacterPage : UIInventoryPage
    {

        public RectTransform inventoryDescription;
        public UIInventoryDescription itemDescription;
        public ItemActionPanel horizontalActionPanel;

        /// <summary>
        /// Hides Inventory - and disables the mouse follower - resets item description
        /// </summary>
        public override void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            inventoryDescription.gameObject.SetActive(false);
            itemDescription.ResetDescription();
            UILayer = LayerMask.NameToLayer("UI");
        }

        /// <summary>
        /// Starts up the inventory and it initialiues the items
        /// </summary>
        /// <param name="inventorySize"></param>
        public override void InitializeInventoryUI(int inventorySize)
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

        public override void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public override void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        public override void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            InvokeOnItemActionRequested(index);
        }


        public override void HandleEndDrag(UIInventoryItem inventoryItemUI)
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

        public override void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        public override void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1 || currentlyDraggedItemIndex == -1)
            {
                return;
            }
            InvokeOnSwapItems(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        public override void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        public override void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            //HandleItemSelection(inventoryItemUI);
            InvokeOnStartDragging(index);
        }

        public override void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        public override void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            inventoryDescription.gameObject.SetActive(true);
            InvokeOnDescriptionRequested(index);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();

        }

        public override void ResetSelection()
        {
            inventoryDescription.gameObject.SetActive(false);
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public override void AddActionHorizontal(string actionName, Action performAction)
        {
            horizontalActionPanel.AddButton(actionName, performAction);
        }

        public override void RefreshAction()
        {
            horizontalActionPanel.RemoveOldButtons();
        }

        public override void ShowHorizontalItemAction(int itemIndex)
        {
            horizontalActionPanel.Toggle(true);
        }

        public override void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
            horizontalActionPanel.Toggle(false);
        }

        public override void Hide()
        {
            horizontalActionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }





        //Returns 'true' if we are touching or hovering on Unity UI element.
        public override bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }


        //Returns 'true' if we are touching or hovering on Unity UI element.
        public override bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
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
        public override List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}