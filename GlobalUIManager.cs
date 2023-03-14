using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class GlobalUIManager : Singleton<GlobalUIManager>
    {
        private GlobalUICanvas rootPrefab;
        [NonSerialized] public GlobalUICanvas root;
        private readonly List<ViewBase> cachedViews = new();

        void Start()
        {
            rootPrefab = ResourceUtil.Load<GameObject>(PathUtil.GlobalPanel("GlobalCanvas")).GetComponent<GlobalUICanvas>();
            GameObject rGob = Instantiate(rootPrefab.gameObject);
            root = rGob.GetComponent<GlobalUICanvas>();
            root.gameObject.name = "Global UI";
            DontDestroyOnLoad(rGob);
        }

        private T InstGet<T>() where T : ViewBase
        {
            foreach(var view in cachedViews)
            {
                if(view is T viewCast)
                    return viewCast;
            }
            return null;
        }

        private T InstLoad<T>() where T : ViewBase
        {
            T rv = InstGet<T>();
            if (rv != null) return rv;

            string name = typeof(T).Name;

            ViewBase v = UIUtil.GlobalLoadViewFunc(name);
            if(v is not T tv)
            {
                Debug.LogError($"View Prefab did not have required component : {name}");
                return null;
            }

            GameObject gob = Instantiate(tv.gameObject);
            rv = gob.GetComponent<T>();
            rv.isGlobal = true;
            cachedViews.Add(rv);
            return rv;
        }

        private void SetLayer(ViewBase v, UILayer layer) 
        {
            Transform p = layer switch
            {
                UILayer.Top => root.layerTop,
                UILayer.Bottom => root.layerBottom,
                UILayer.Common => root.layerCommon,
                _ => root.transform,
            };
            if(v != null && p != null)
            {
                v.transform.SetParent(p, false);

            }

            var parent = v.transform.parent;
            if (parent != null)
            {
                v.RectTransform.SetSiblingIndex(parent.childCount);
            }
        }

        private T InstOpen<T>(T v, UILayer layer) where T : ViewBase
        {
            v = v == null ? InstLoad<T>() : v;

            SetLayer(v, layer);
            if(!v.gameObject.activeSelf) v.gameObject.SetActive(true);

            v.OnOpen();
            return v;
        }

        private void InstClose<T>(T v = null) where T:ViewBase
        {
            v = v == null ? InstGet<T>() : v;
            v.OnClose();
            v.gameObject.SetActive(false);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void RTInit()
        {
            Instance.Dummy();
        }
        public static T Load<T>() where T : ViewBase => Instance.InstLoad<T>();
        public static T Get<T>() where T : ViewBase => Instance.InstGet<T>();
        public static T Open<T>(UILayer layer = UILayer.Common, T v = null) where T : ViewBase => Instance.InstOpen(v, layer);
        public static void Close<T>(T self) where T : ViewBase => Instance.InstClose(self);
    }

