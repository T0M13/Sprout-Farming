using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Inventory
{
    public class CharacterInventoryController : InventoryController
    {

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


        private void Update()
        {
            if (inventoryUI == null) return;
            if (_playerInventory.action.triggered)
                ToggleInventory();
        }

    }
}