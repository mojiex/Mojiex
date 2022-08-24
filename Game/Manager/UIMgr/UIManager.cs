using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/20
    public class UIManager : IMgr
    {
        private List<UIObject> m_uiObjects = new List<UIObject>();
        private bool inited = false;

        private Transform UITransform;
        private Transform UIRoot;
        private Camera uiCam;
        public void Init()
        {
            if (IsInited())
            {
                return;
            }
            inited = true;
            MDebug.Log("ui");
            CreateUITransform();
        }

        private void CreateUITransform()
        {
            GameObject uiPrefab = GameObject.Instantiate(Resources.Load<GameObject>("UITransform"));
            GameObject.DontDestroyOnLoad(uiPrefab);
            UITransform = uiPrefab.transform;
            UIRoot = UITransform.Find("UIRoot");
            uiCam = UITransform.Find("UIcamera").GetComponent<Camera>();
            MDebug.Log("Created");
        }

        public bool IsInited()
        {
            return inited;
        }

        public void Dispose()
        {
            m_uiObjects.Clear();
            GameObject.Destroy(UITransform.gameObject);
        }

        /// <summary>
        /// 按esc后根据当前UI是否可见及其是否可以被关闭进行相关操作，有UI关闭返回true，否则返回false
        /// </summary>
        public bool EscCloseUI()
        {
            for (int i = m_uiObjects.Count - 1; i >= 0; i--)
            {
                if (m_uiObjects[i].EscClose && m_uiObjects[i].m_go.activeInHierarchy)
                {
                    m_uiObjects[i].Close();
                    m_uiObjects.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public T Add<T>() where T:UIObject,new()
        {
            T script = new T();
            GameObject gameObject = GetPrefab(typeof(T));
            script.Init(gameObject);
            return default(T);
        }

        private GameObject GetPrefab(Type type)
        {

            return new GameObject();
        }
    }
}