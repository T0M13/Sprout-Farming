using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "Equippable Item", menuName = "ScriptableObjects/Items/Equippable Item")]

    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public string ActionName => "Equip";

        public bool PerfomAction(GameObject character, List<ItemParameter> itemState = null)
        {
            AgentItem itemSystem = character.GetComponent<AgentItem>();
            if (itemSystem != null)
            {
                itemSystem.SetItem(this, itemState == null ? DefaultParametersList : itemState);
                return true;
            }
            return false;
        }
    }
}