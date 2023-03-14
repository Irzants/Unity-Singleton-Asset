using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

    public class ItemThumbnail : MonoBehaviour
    {
        [NonSerialized] public ItemCatalog.Item itemMeta;

        public Image itemIcon;
        public TextMeshProUGUI itemPrice;
        public Button button;
        public Image purchaseIcon;

        [Header("Icon type")]
        public Sprite ad;
        public Sprite coin;
        public Sprite jewel;

        public void ApplyMetadata()
        {
            itemIcon.sprite = itemMeta.icon;
            itemPrice.text = $"{itemMeta.price}";
            switch (itemMeta.type)
            {
                case ItemCatalog.PurchaseType.AD:
                    purchaseIcon.sprite = ad;
                    itemPrice.text = $"Watch Ad";
                    break;
                case ItemCatalog.PurchaseType.Coin:
                    purchaseIcon.sprite = coin;
                    break;
                case ItemCatalog.PurchaseType.Jewel:
                    purchaseIcon.sprite = jewel;
                    break;
                default:
                    break;
            }
        }
    }
