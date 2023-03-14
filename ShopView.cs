using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopView : ViewBase
    {
        public List<ItemThumbnail> listItem = new();

        public ItemThumbnail templateItem;

        [Header("Grid")]
        public Transform gridItemParent;

        [Header("Notif Description")]
        [SerializeField] private GameObject descPopup;
        [SerializeField] private TextMeshProUGUI descItem;
        [SerializeField] private TextMeshProUGUI priceItem;
        [SerializeField] private Image prevItem;
        [SerializeField] private Image iconPurchase;

        [Header("Icon type")]
        public Sprite ad;
        public Sprite coin;
        public Sprite jewel;

        private ItemCatalog.Item itemWillBeBought;
        private void CreateThumbnailFood()
        {
            templateItem.gameObject.SetActive(false);
            listItem.Clear();
            var items = ItemCatalog.Instance.items;
            foreach (var item in items)
            {
                GameObject Obj = Instantiate(templateItem.gameObject);
                ItemThumbnail fthumb = Obj.GetComponent<ItemThumbnail>();
                fthumb.itemMeta = item;
                fthumb.ApplyMetadata();
                listFood.Add(fthumb);
                fthumb.button.onClick.AddListener(() =>
                {
                    ShowFoodDesc(item);
                });
                foodObj.transform.SetParent(fridItemParent);
                foodObj.SetActive(true);
            }
        }
        private void Awake()
        {
            CreateThumbnailFood();
        }

        private void ShowFoodDesc(ItemCatalog.Item data)
        {
            descPopup.SetActive(true);
            itemWillBeBought = data;
            prevItem.sprite = data.icon;
            descItem.text = $"{data.itemDesc}";
            priceFood.text = $"{data.price}";
            switch (data.type)
            {
                case ItemCatalog.PurchaseType.AD:
                    iconPurchase.sprite = ad;
                    priceItem.text = $"Watch Ad";
                    break;
                case ItemCatalog.PurchaseType.Coin:
                    iconPurchase.sprite = coin;
                    break;
                case ItemCatalog.PurchaseType.Jewel:
                    iconPurchase.sprite = jewel;
                    break;
                default:
                    break;
            }
        }

        public void ClickCancel()
        {
            descPopup.SetActive(false);

        }
        public void ClickBuy()
        {
            descPopup.SetActive(false);

        }

        internal override void OnOpen()
        {
            base.OnOpen();
        }

        public void OnClickClose()
        {
            base.Close();
        }
    }
