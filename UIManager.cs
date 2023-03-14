using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class UIManager : SceneSingleton<UIManager>
    {
        public RectTransform bottomLayer;
        public RectTransform commonLayer;
        public RectTransform topLayer;

        private readonly List<ViewBase> mCachedViews = new List<ViewBase>();
        private void Awake()
        {
            _instance = this;
        }

        private T GetView<T>() where T : ViewBase
        {
            foreach (var p in mCachedViews)
            {
                if (p is T t)
                {
                    return t;
                }
            }
            return null;
        }

        private void SetLayer(ViewBase view, UILayer layer)
        {
            if (view == null)
                return;

            if (layer == UILayer.Common && commonLayer != null)
            {
                view.transform.SetParent(commonLayer, false);
            }
            else if (layer == UILayer.Top && topLayer != null)
            {
                view.transform.SetParent(topLayer, false);
            }
            else if (layer == UILayer.Bottom && bottomLayer != null)
            {
                view.transform.SetParent(bottomLayer, false);
            }

            var parent = view.transform.parent;
            if (parent != null)
            {
                view.RectTransform.SetSiblingIndex(parent.childCount);
            }
        }

        private T InstLoad<T>() where T : ViewBase
        {
            var view = GetView<T>();
            if (view != null)
            {
                return view;
            }

            string typeName = typeof(T).Name;

            view = (T)UIUtil.LoadViewFunc(typeName);
            if (view == null)
            {
                Debug.LogError("View Load Failed: " + typeName);
                return null;
            }

            view = Instantiate(view);
            view.gameObject.SetActive(false);
            view.isGlobal = false;
            mCachedViews.Add(view);
            return view;
        }

        private T InstOpen<T>(UILayer layer = UILayer.Common) where T : ViewBase
        {
            T view = GetView<T>();

            if (view == null)
            {
                view = Load<T>();
            }
            return InstOpen(view, layer);
        }

        private T InstOpen<T>(T view, UILayer layer) where T : ViewBase
        {
            SetLayer(view, layer);
            if (!view.gameObject.activeSelf)
            {
                view.gameObject.SetActive(true);
            }

            view.OnOpen();

            return view as T;
        }

        private void InstClose<T>() where T : ViewBase
        {
            InstClose(GetView<T>());
        }

        private void InstClose<T>(T self) where T : ViewBase
        {
            self.gameObject.SetActive(false);
            self.OnClose();
        }

        #region API
        public static T Load<T>() where T : ViewBase => Instance.InstLoad<T>();

        public static T Open<T>(UILayer layer = UILayer.Common) where T : ViewBase => Instance.InstOpen<T>(layer);
        public static T Open<T>(T self, UILayer layer = UILayer.Common) where T : ViewBase => Instance.InstOpen<T>(self, layer);

        public static void Close<T>() where T : ViewBase => Instance.InstClose<T>();
        public static void Close<T>(T self) where T : ViewBase => Instance.InstClose<T>(self);

        public static T Find<T>() where T : ViewBase => Instance.GetView<T>();
        #endregion
    }

