using Inventory.Model;
using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentItem : MonoBehaviour
{
    [SerializeField]
    private EquippableItemSO equippableItem;

    [SerializeField]
    private CharacterInventory inventory;
    [SerializeField] private InventorySO inventoryData;

    [SerializeField]
    private List<ItemParameter> parametersToModify, itemCurrentState;

    private void Awake()
    {
        inventory = GetComponent<CharacterInventory>();
        inventoryData = inventory.inventoryData;
        if (inventory == null) Debug.LogWarning("Inventory is missing");
    }

    public void SetItem(EquippableItemSO equippableItemSO, List<ItemParameter> itemState)
    {
        if (equippableItem != null)
        {
            inventoryData.AddItem(equippableItem, 1, itemCurrentState);
        }
        this.equippableItem = equippableItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }

    private void ModifyParameters()
    {
        foreach (var parameter in parametersToModify)
        {
            if (itemCurrentState.Contains(parameter))
            {
                int index = itemCurrentState.IndexOf(parameter);
                float newValue = itemCurrentState[index].value + parameter.value;
                itemCurrentState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue
                };
            }
        }
    }
}
