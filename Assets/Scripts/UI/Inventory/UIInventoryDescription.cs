using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {

        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;


        private void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {            
            itemImage.gameObject.SetActive(false);
            title.text = "";
            description.text = "";
        }

        public void SetDescription(Sprite sprite, string itemName, string itemDesciption)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            itemImage.preserveAspect = true;
            title.text = itemName;
            description.text = itemDesciption;
        }

    }
}