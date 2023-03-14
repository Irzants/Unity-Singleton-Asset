using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodCatalog : SingletonAsset<FoodCatalog>
    {
        /// <summary>
        /// Class for call scriptable object which inside catalog
        /// </summary>
        [Serializable]
        public class Food
        {
            public string id;
            public Sprite icon;
            public float price;
            public PurchaseType type;
            [TextArea]
            public string foodDesc;
        }

        public enum PurchaseType
        {
            AD,
            Coin,
            Jewel
        }

        public List<Food> foods;

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
