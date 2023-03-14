using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemCatalog : SingletonAsset<FoodCatalog>
    {
        /// <summary>
        /// Class for call scriptable object which inside catalog
        /// </summary>
        [Serializable]
        public class Item
        {
            public string id;
            public Sprite icon;
            public float price;
            public PurchaseType type;
            [TextArea]
            public string itemDesc;
        }

        public enum PurchaseType
        {
            AD,
            Coin,
            Jewel
        }

        public List<Item> items;

        /// <summary>
        /// Auto create catalog in editor
        /// </summary>
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        public static void EditorInit()
        {
            EnsureExists();
        }
#endif
    }
