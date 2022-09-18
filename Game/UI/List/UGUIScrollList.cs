#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/1
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class UGUIScrollList : UGUIList
    {
        #region scroll limiter

        private ScrollLimiterList m_scrollrect;
        [SerializeField]
        private RectTransform m_viewPort;

        public bool Interactable
        {
            get => interactable;
            set
            {
                if (m_scrollrect)
                {
                    m_scrollrect.interactable = value;
                    interactable = value;
                }
            }
        }
        [SerializeField]
        private bool interactable = true;

        public bool Horizon
        {
            get => horizon;
            set
            {
                if (m_scrollrect)
                {
                    m_scrollrect.Horizon = value;
                    horizon = value;
                }
            }
        }
        [SerializeField]
        private bool horizon = true;

        public bool Vertical
        {
            get => vertiacl;
            set
            {
                if (m_scrollrect)
                {
                    m_scrollrect.Vertical = value;
                    vertiacl = value;
                }
            }
        }
        [SerializeField]
        private bool vertiacl = true;

        public float Step
        {
            get => step;
            set
            {
                step = Mathf.Clamp01(value);
                if (m_scrollrect)
                {
                    m_scrollrect.step = step;
                }
            }
        }
        [Header("动画相关")]
        [Range(0, 1)]
        [SerializeField]
        private float step = 0.1f;

        public float StopDistance
        {
            get => stopDistance;
            set
            {
                stopDistance = Mathf.Max(0, value);
                if (m_scrollrect)
                {
                    m_scrollrect.StopDistance = stopDistance;
                }
            }
        }
        [SerializeField]
        [Tooltip("如果该属性值小于等于0则使用默认值，默认值为当前激活的方向的Gap最小值的千分之一")]
        private float stopDistance = -1;

        public bool IsClamp
        {
            get => isClamp;
            set
            {
                isClamp = value;
                if (m_scrollrect)
                {
                    m_scrollrect.IsClamp = isClamp;
                }
            }
        }
        [SerializeField]
        private bool isClamp = true;
        #endregion

        #region List
        public new object[] Data
        {
            get => base.Data;
            set
            {
                base.Data = value;
                if (m_scrollrect && value.Length > 0)
                {
                    m_scrollrect.ItemCount = value.Length;
                    if(m_scrollrect.GetSelected() == -1)
                        SelectItem(0);
                }
            }
        }

        #endregion

        public System.Action<int> OnClickItem;
        private void Awake()
        {
            if(TryGetComponent(out ScrollLimiterList scroll))
            {
                m_scrollrect = scroll;
            }
            else
            {
                m_scrollrect = gameObject.AddComponent<ScrollLimiterList>();
                m_scrollrect.interactable = Interactable;
                m_scrollrect.Horizon = Horizon;
                m_scrollrect.Vertical = Vertical;
                m_scrollrect.step = Step;
                m_scrollrect.IsClamp = IsClamp;
            }
            m_scrollrect.Item = Prefab != null ? Prefab.gameObject : null;
            m_scrollrect.ViewPort = m_viewPort;
            if(StopDistance > 0)
            {
                m_scrollrect.StopDistance = StopDistance;
            }
            //m_scrollrect.Awake();
        }

        public void SelectItem(int index, System.Action onComplete = null)
        {
            if(m_scrollrect == null)
            {
                throw new NullReferenceException("Scrollrect is null");
            }
            if (m_scrollrect.GetSelected() != index)
            {
                m_scrollrect.Select(index);
            }
            else
            {
                MDebug.Log("Selected:" + index);
                OnClickItem?.Invoke(index);
            }
        }
    }
}