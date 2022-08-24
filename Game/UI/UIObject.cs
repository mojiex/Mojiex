using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/20
    public class UIObject
    {
        public GameObject m_go;
        public bool isSystemUI = false;
        public bool neediOSScreenFix = false;
        public bool EscClose { get; set; } = false;
        public int forceDepth = -1;
        private EUILayer currentSortLayer = EUILayer.Common;
        private int LayerPriority = 0;

        public UnityEngine.Events.UnityEvent onDestroy = new UnityEngine.Events.UnityEvent();

        public virtual int GetCurrentSortLayer()
        {
            return (int)currentSortLayer * 1000 + LayerPriority;
        }

        public EUILayer GetUILayer() => currentSortLayer;
        public void SetCurrentSortLayer(int layer)
        {
            LayerPriority = layer % ((int)currentSortLayer);
            if (m_go != null)
            {
                if (m_go.GetComponent<Canvas>() != null)
                {
                    m_go.GetComponent<Canvas>().sortingOrder = layer;
                }
            }
        }

        public virtual void Init(GameObject gameObject) { m_go = gameObject; }

        public virtual void Hide() { m_go.gameObject.SetActive(false); }

        public virtual void Show() { m_go.gameObject.SetActive(true); }

        public virtual void Close()
        {
            onDestroy?.Invoke();
            onDestroy.RemoveAllListeners();
            GameObject.Destroy(m_go);
        }
    }

    public enum EUILayer
    {
        Common,
        Top,
        Pop,
    }
}