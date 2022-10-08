using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UIInventoryChestPage : UIInventoryPage
    {
        public override void Awake()
        {
            //base.Awake();
        }

        public void CustomAwake()
        {
            Hide();
            mouseFollower.Toggle(false);
            UILayer = LayerMask.NameToLayer("UI");
        }

    }
}