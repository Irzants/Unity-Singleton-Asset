using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance => _instance == null ? (_instance = CreateNewInstance()) : _instance;

        protected void Dummy() { }

        private static bool _isQuitting = false;
        private static bool _isQuittingAttached = false;

        public static bool IsQuitting => _isQuitting;

        // Prevent new instance being emitted
        private static void SingletonBaseOnQuit()
        {
            _isQuitting = true;
        }

        private static GameObject GetParent()
        {
            if (_isQuitting) return null;
            const string pTitle = "Singletons";
            GameObject parent= GameObject.Find(pTitle);
            if (parent == null)
            {
                parent = new(pTitle);
                DontDestroyOnLoad(parent);
            }
            return parent;
        }

        private static T CreateNewInstance()
        {
            if (_isQuitting) return null;
            GameObject parent = GetParent();

            string objName = typeof(T).Name;
            StringBuilder sb = new();

            for(int i =0; i < objName.Length; i++)
            {
                char c = objName[i];
                if (c >= 'A' && c <= 'Z' && i > 0)
                    sb.Append(' ');
                sb.Append(c);
            }

            if (!_isQuittingAttached)
            {
                Application.quitting += SingletonBaseOnQuit;
                _isQuittingAttached = true;
            }

            GameObject obj = new(sb.ToString());
            obj.transform.SetParent(parent.transform.transform, false);
            return obj.AddComponent<T>();
        }

    }

