using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Mojiex
{
    //CreateTime : 2022/8/20
    public class UIManager : IMgr
    {
        private List<UIObject> m_uiObjects = new List<UIObject>();
        private List<UIObject> maskUIs = new List<UIObject>();
        private bool inited = false;

        private Transform UITransform;
        private Transform UIRoot;
        private Camera uiCam;
        private GameObject mask;

        public bool EnabeleEscCheck
        {
            get => _enabeleEscCheck;
            set
            {
                if (_enabeleEscCheck == value)
                    return;
                _enabeleEscCheck = value;
                EscCheckChanged(value);
            }
        }
        private bool _enabeleEscCheck = true;
        public class UIEvents : UnityEngine.Events.UnityEvent { }
        public UIEvents uiDefaultEscEvent = new UIEvents();
        public UIEvents onClosePanel = new UIEvents();
        public UIEvents onAddPanel = new UIEvents();
        public void Init()
        {
            if (IsInited())
            {
                return;
            }
            inited = true;
            MDebug.Log("[UI:Init]");
            CreateUITransform();
            EscCheckChanged(_enabeleEscCheck);
            uiDefaultEscEvent.AddListener(() => MDebug.Log("UIDefaultEscEvent"));
        }

        private void CreateUITransform()
        {
            GameObject uiPrefab = GameObject.Instantiate(Resources.Load<GameObject>("UITransform"));
            GameObject.DontDestroyOnLoad(uiPrefab);
            UITransform = uiPrefab.transform;
            UIRoot = UITransform.Find("UIRoot");
            uiCam = UITransform.Find("UIcamera").GetComponent<Camera>();
            MDebug.Log("[UI:Created]");
        }

        public bool IsInited()
        {
            return inited;
        }

        public void Dispose()
        {
            m_uiObjects.Clear();
            maskUIs.Clear();
            uiDefaultEscEvent.RemoveAllListeners();
            GameObject.Destroy(UITransform.gameObject);
        }

        #region Logic
        private void EscCheckChanged(bool state)
        {
            if (!state)
            {
                SupportBehavior.Inst.RemoveUpdateMethod(EscCheckLogic);
            }
            else
            {
                SupportBehavior.Inst.AddUpdateMethod(EscCheckLogic);
            }
        }

        private void EscCheckLogic()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !EscCloseUI())
            {
                uiDefaultEscEvent?.Invoke();
            }
        }
        #endregion

        /// <summary>
        /// 按esc后根据当前UI是否可见及其是否可以被关闭进行相关操作，有UI关闭返回true，否则返回false
        /// </summary>
        public bool EscCloseUI()
        {
            for (int i = m_uiObjects.Count - 1; i >= 0; i--)
            {
                if (m_uiObjects[i].EscClose && m_uiObjects[i].m_go.activeInHierarchy)
                {
                    m_uiObjects[i].escCloseEvents.Invoke();
                    Close(m_uiObjects[i]);
                    return true;
                }
            }
            return false;
        }

        public T Add<T>() where T : UIObject, new()
        {
            for (int i = 0; i < m_uiObjects.Count; i++)
            {
                if (m_uiObjects[i].GetType() == typeof(T))
                {
                    m_uiObjects[i].Show();
                    return (T)m_uiObjects[i];
                }
            }
            T script = new T();
            GameObject uigameObject = GetPrefab(typeof(T));
            int layer = script.GetCurrentSortLayer();
            CreateCanvas(uigameObject, layer);
            script.Init(uigameObject);

            m_uiObjects.Add(script);
            onAddPanel.Invoke();
            return script;
        }

        public void Close<T>() where T : UIObject
        {
            for (int i = 0; i < m_uiObjects.Count; i++)
            {
                if (m_uiObjects[i].GetType() == typeof(T))
                {
                    m_uiObjects[i].Close();
                    CloseMask(m_uiObjects[i]);
                    m_uiObjects.RemoveAt(i);
                    onClosePanel.Invoke();
                    return;
                }
            }
        }

        private void Close(UIObject uIObject)
        {
            if (m_uiObjects.Contains(uIObject))
            {
                uIObject.Close();
                CloseMask(uIObject);
                m_uiObjects.Remove(uIObject);
                return;
            }
        }
        public T Get<T>() where T : UIObject
        {
            for (int i = 0; i < m_uiObjects.Count; i++)
            {
                if (m_uiObjects[i].GetType() == typeof(T))
                {
                    return (T)m_uiObjects[i];
                }
            }
            return null;
        }

        public void AddMask(UIObject uiObj)
        {
            CreateMask(uiObj.GetCurrentSortLayer() - 1);
            maskUIs.Add(uiObj);
        }
        public void CloseMask(UIObject uiObj)
        {
            maskUIs.Remove(uiObj);
            if (maskUIs.Count == 0 && mask != null)
            {
                mask.SetActive(false);
            }
            else if(maskUIs.Count > 0)
            {
                CreateMask(maskUIs[maskUIs.Count - 1].GetCurrentSortLayer() - 1);
            }
        }

        public GameObject CreateMask(int layer)
        {
            if (mask != null)
            {
                mask.GetComponent<Canvas>().sortingOrder = layer;
                mask.SetActive(true);
                return mask;
            }
            else
            {
                mask = new GameObject("Mask");
                var rectTransform = mask.AddComponent<RectTransform>();
                rectTransform.localScale = Vector3.one;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.sizeDelta = Vector2.zero;
                rectTransform.localPosition = Vector3.zero;
                Image img = mask.AddComponent<Image>();
                img.color = new Color(0, 0, 0, 0.52f);
                //img.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                img.raycastTarget = true;
                AddToRoot(mask, GetLayer());
                CreateCanvas(mask, layer);
                return mask;
            }
        }

        private void AddToRoot(GameObject ui, int layer)
        {
            RectTransform rt = ui.transform.GetComponent<RectTransform>();
            Vector2 originOffsetMax = Vector2.zero, originOffsetMin = Vector2.zero;
            if (rt != null)
            {
                originOffsetMax = rt.offsetMax;
                originOffsetMin = rt.offsetMin;
            }
            ui.transform.SetParent(UIRoot);
            if (rt != null)
            {

                rt.offsetMax = originOffsetMax;
                rt.offsetMin = originOffsetMin;
            }
            ui.transform.localPosition = Vector3.zero;
            ui.transform.localScale = Vector3.one;
            ui.IterateGameObject((go) =>
            {
                if (go.layer == LayerMask.NameToLayer("Default"))
                {
                    go.layer = layer;
                }
            });
        }
        private int GetLayer()
        {
            return LayerMask.NameToLayer("UI");
        }
        private GameObject GetPrefab(Type type)
        {
            string prefabName = type.Name;
            return GameObject.Instantiate(Resources.Load<GameObject>(prefabName), GetUIRoot());
        }
        private void CreateCanvas(GameObject r, int layer)
        {
            Canvas canvs;
            if (!r.TryGetComponent(out canvs))
            {
                canvs = r.AddComponent<Canvas>();
            }
            canvs.overrideSorting = true;
            canvs.renderMode = RenderMode.ScreenSpaceCamera;
            canvs.worldCamera = uiCam;
            canvs.sortingOrder = layer;
            LayerMask layerMask = new LayerMask();
            layerMask.value = LayerMask.GetMask("UI");
            r.GetMissingComponent<UIGraphicRaycaster>().SetLayerMask(layerMask);
            r.GetMissingComponent<UIGraphicRaycaster>().ignoreReversedGraphics = false;
        }

        public Type GetTopPanelType()
        {
            if(m_uiObjects.Count == 0)
            {
                return null;
            }
            return m_uiObjects[m_uiObjects.Count - 1].GetType();
        }

        public Transform GetUIRoot() => UIRoot;
        public Camera GetUICamera() => uiCam;
        public Rect GetUIRect() => UIRoot.RectTransform().rect;
    }
}