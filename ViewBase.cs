using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum UILayer
    {
        Common = 0,
        Top = 1,
        Bottom = 2,
    }

    public abstract class ViewBase : MonoBehaviour
    {
        protected RectTransform mRectTransform;
        [NonSerialized] public bool isGlobal;
        public RectTransform RectTransform => gameObject.GetComponentCached(ref mRectTransform);

        internal virtual void OnOpen() { }
        internal virtual void OnClose() { }
        public void Close()
        {
            Action<ViewBase> ms = (isGlobal ? GlobalUIManager.Close : UIManager.Close);
            ms(this);
        }
    }

