using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FluffyUnderware.DevTools;
using System;

namespace Mojiex.AnimateUI
{
    [Serializable]
    public class AnimateSlider : Slider
    {
        public enum ValueAnimChangeType
        {
                ///强制结束上一个动画
            ForceComplete,
                ///合并动画
            Combine,
        }
        [Tooltip("值为true时会暂停动画")]
        public bool AnimationPause = false;
        public float AnimateTime = 0.2f;
        private float AnimateTimeCounter = 0.0f;
        public DTTween.EasingMethod EasingMethod = DTTween.EasingMethod.Linear;
        public ValueAnimChangeType valueAnim = ValueAnimChangeType.Combine;

        public override float value
        { 
            get => base.value; 
            set 
            {
                switch (valueAnim)
                {
                    case ValueAnimChangeType.ForceComplete:
                        ForceComplete();
                        break;
                    case ValueAnimChangeType.Combine:
                        AnimateTimeCounter = 0;
                        break;
                }
                m_startPos = base.value;
                m_targetPos = value;
            }
        }
        private float m_startPos;
        [SerializeField]
        private float m_targetPos;

        protected override void Awake()
        {
            base.Awake();
            //必须允许小数存在
            wholeNumbers = false;
            m_startPos = m_targetPos = base.value;
        }
        protected override void Update()
        {
            base.Update();
            if(!AnimationPause && base.value != m_targetPos)
            {
                AnimateTimeCounter += Time.deltaTime;
                if(UpdatePos(m_startPos,m_targetPos,AnimateTimeCounter/AnimateTime,out float res))
                {
                    base.value = res;
                }
                else
                {
                    base.value = m_targetPos;
                    AnimateTimeCounter = 0;
                }
            }
        }
        ///强制结束当前动画
        public bool ForceComplete()
        {
            if (base.value != m_targetPos)
            {
                base.value = m_targetPos;
                AnimateTimeCounter = 0;
                return true;
            }
            return false;
        }
        private bool UpdatePos(float startValue, float endValue, float pos, out float res)
        {
            res = 0;
            if (pos > 1 || pos < 0)
            {
                return false;
            }
            res = DTTween.Ease(EasingMethod, pos, startValue, endValue - startValue);
            return true;
        }
    }
}