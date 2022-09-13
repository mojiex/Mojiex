#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/1
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Mojiex
{
    [DisallowMultipleComponent]
    public class ScrollLimiterRect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /*
         * list : require LayoutElement控制面板尺寸
         * m_rect : 需要mask
         */
        private RectTransform m_RectTransform;
        private Vector2 m_currentPos;
        protected Vector2 m_targetPos;
        protected bool updatePos = false;
        protected Vector2 m_init;

        protected float minX, minY, maxX, maxY;

        public bool interactable = true;
        public bool IsLimit = true;

        public RectTransform ViewPort
        {
            get => m_viewPort;
            set => m_viewPort = value;
        }
        [SerializeField]
        private RectTransform m_viewPort;
        [Tooltip("限制面板移动范围")]
        public bool IsClamp = true;
        [Header("面板方向")]
        public bool Horizon = true;
        public bool Vertical = true;

        public int HorizonGap = 300;
        public int VerticalGap = 300;

        [Header("动画相关")]
        [Range(0, 1)]
        public float step = 0.1f;
        [Tooltip("如果该属性值小于等于0则使用默认值，默认值为当前激活的方向的Gap最小值的千分之一")]
        public float StopDistance = -1;

        public virtual void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            m_RectTransform = transform as RectTransform;
            if (StopDistance <= 0 && (Horizon || Vertical))
            {
                int v = Vertical ? VerticalGap : int.MaxValue;
                int h = Horizon ? HorizonGap : int.MaxValue;

                StopDistance = Mathf.Min(v, h) / 1000.0f;
            }
            //Vector2 pos = m_RectTransform.position;
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(m_RectTransform);
            
            UpdateInitPos();
        }
        public void UpdateInitPos()
        {
            SetRange();
            if (IsClamp)
            {
                Vector2 initPos = m_RectTransform.anchoredPosition;
                initPos.x = Horizon ? maxX : initPos.x;
                initPos.y = Vertical ? maxY : initPos.y;
                m_RectTransform.anchoredPosition = initPos;
            }
            m_currentPos = m_init = m_RectTransform.anchoredPosition;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!interactable)
                return;
            updatePos = false;
            if (IsClamp)
                SetRange();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                float dragValue = 1.0f;
                Vector2 pos = m_RectTransform.anchoredPosition;
                //Vector2 pos = m_RectTransform.position;
                //Debug.Log(eventData.delta);
                if (Horizon)
                {
                    dragValue = pos.x < maxX ? dragValue : Mathf.Clamp(20 / (pos.x - maxX), 0.001f, 1.0f);
                    dragValue = minX < pos.x ? dragValue : Mathf.Clamp(20 / (minX - pos.x), 0.001f, 1.0f);
                    pos.x += eventData.delta.x * dragValue;
                }
                if (Vertical)
                {
                    dragValue = pos.y < maxY ? dragValue : Mathf.Clamp(20 / (pos.y - maxY), 0.001f, 1.0f);
                    dragValue = minY < pos.y ? dragValue : Mathf.Clamp(20 / (minY - pos.y), 0.001f, 1.0f);
                    pos.y += eventData.delta.y * dragValue;
                }
                m_RectTransform.anchoredPosition = pos;
                //m_RectTransform.position = pos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if ((!IsLimit) || (!interactable))
            {
                return;
            }
            Vector2 pos = m_RectTransform.anchoredPosition;
            //Debug.Log(pos);
            //Vector2 pos = m_RectTransform.position;
            //int sign; --2
            m_currentPos = pos;
            if (Horizon)
            {
                //sign = pos.x >= 0 ? 1 : -1; --2
                //pos.x -= (int)pos.x % HorizonGap; --1
                //pos.x = (((int)pos.x + (HorizonGap / 2) * sign) / HorizonGap) * HorizonGap + m_init.x; --2
                pos.x = GetLimitedPos(pos.x, m_init.x, HorizonGap);
                if (IsClamp)
                    pos.x = ClampX(pos.x);
            }
            if (Vertical)
            {
                //sign = pos.y >= 0 ? 1 : -1; --2
                //pos.y -= (int)pos.y % VerticalGap; --1
                //pos.y = ((int)pos.y + (VerticalGap / 2) * sign) / VerticalGap * VerticalGap + m_init.y; --2
                pos.y = GetLimitedPos(pos.y, m_init.y, VerticalGap);
                if (IsClamp)
                    pos.y = ClampY(pos.y);
            }
            m_targetPos = pos;
            updatePos = true;
        }

        protected virtual void FixedUpdate()
        {
            if (!interactable)
                return;
            if (updatePos
                && m_targetPos != m_currentPos
                && StopDistance > 0)
            {
                m_currentPos = Vector2.Lerp(m_currentPos, m_targetPos, step);
                if (Vector2.Distance(m_targetPos, m_currentPos) <= StopDistance)
                {
                    m_currentPos = m_targetPos;
                    updatePos = false;
                }
                m_RectTransform.anchoredPosition = m_currentPos;
                //m_RectTransform.position = m_currentPos;
            }
        }

        protected void SetRange()
        {
            if (m_viewPort == null)
            {
                MDebug.LogError("ViewPort is null,please set it and try again");
                IsClamp = false;
                return;
            }
            Debug.Log(m_RectTransform.rect.width + "," + m_RectTransform.rect.height);
            float tmaxX = m_viewPort.anchoredPosition.x + m_viewPort.pivot.x * m_viewPort.rect.width - m_RectTransform.rect.width * m_RectTransform.pivot.x;
            float tminX = m_viewPort.anchoredPosition.x - (1 - m_viewPort.pivot.x) * m_viewPort.rect.width + m_RectTransform.rect.width * (1 - m_RectTransform.pivot.x);
            float tmaxY = m_viewPort.anchoredPosition.y + m_viewPort.pivot.y * m_viewPort.rect.height - m_RectTransform.rect.height * m_RectTransform.pivot.y;
            float tminY = m_viewPort.anchoredPosition.y - (1 - m_viewPort.pivot.y) * m_viewPort.rect.height + m_RectTransform.rect.height * (1 - m_RectTransform.pivot.y);
            maxX = Mathf.Max(tminX, tmaxX);
            minX = Mathf.Min(tminX, tmaxX);
            maxY = Mathf.Max(tminY, tmaxY);
            minY = Mathf.Min(tminY, tmaxY);
            //Debug.Log($"{minX},{maxX},{minY},{maxY}");
            //Debug.Log($"1、{m_RectTransform.rect.width},{m_RectTransform.rect.height},{m_RectTransform.rect.yMin},{m_RectTransform.rect.yMax}");
            //Debug.Log($"2、{m_rect.rect.width},{m_rect.rect.height},{m_rect.rect.yMin},{m_rect.rect.yMax}");
        }

        protected virtual float ClampX(float originX)
        {
            return Mathf.Clamp(originX, minX, maxX);
        }

        protected virtual float ClampY(float originY)
        {
            return Mathf.Clamp(originY, minY, maxY);
        }

        private float GetLimitedPos(float originPos, float initPos, float gap)
        {
            if (originPos == initPos)
            {
                return initPos;
            }
            int value = originPos < initPos ? 1 : -1;
            int i = 0;
            if(originPos < initPos)
            {
                while ((originPos + (i * value + 0.5f) * gap) < initPos)
                {
                    i++;
                }
            }
            else if(originPos > initPos)
            {
                while ((originPos + (i * value + 0.5f) * gap) > initPos)
                {
                    i++;
                }
            }
            
            Debug.Log($"{originPos},{initPos + value * i * gap}");
            return initPos - value * i * gap;
        }

        //private bool UpdatePos(Vector2 startValue,Vector2 endValue,float pos,ref Vector2 res)
        //{
        //    res = Vector2.zero;
        //    if(pos > 1 || pos < 0)
        //    {
        //        MDebug.LogError($"pos : {pos} must in range of [0,1]");
        //        return false;
        //    }
        //    if(pos == 1)
        //    {
        //        return false;
        //    }
        //    res = 
        //}
    }

}