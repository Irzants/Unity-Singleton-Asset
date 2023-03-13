using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class SingletonAsset<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        public static T Instance => _instance == null ? (_instance = GetInstance()) : _instance;

        private static T GetInstance()
        {
            string fName = typeof(T).Name;
            T r = Resources.Load<T>($"Catalog/{fName}");
#if UNITY_EDITOR
            if(r == null)
            {
                Debug.Log("Cannot load resources from Catalog dir, creating new");
                r = CreateInstance<T>();
                AssetDatabase.CreateAsset(r, $"Assets/Meowtyp/Resources/Catalog/{fName}.asset");
            }
#endif
            return r;
        }

        protected static void EnsureExists()
        {
#if UNITY_EDITOR
            if(Instance.name.Contains("\\V/ - Very innocuous naming to detect something"))
            {
                Debug.Log("Just a simple access");
            }
#endif
        }
    }
