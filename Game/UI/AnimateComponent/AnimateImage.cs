#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/24
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mojiex.Ease;

namespace Mojiex.AnimateUI
{
    /// <summary>
    /// 提供fill动画
    /// </summary>
    public class AnimateImage : Image
    {

        private EaseBase ease = new Linear();
	    public new float fillAmount
        {
            get => base.fillAmount;
            set
            {
                m_startPos = base.fillAmount;
                AnimateTimeCounter = 0;
                m_targetPos = Mathf.Clamp01(value);
            }
        }
        
        public float AnimateTime = 0.2f;
        private float AnimateTimeCounter = 0.0f;

        private float m_startPos;
        [SerializeField]
        private float m_targetPos;

        public void SetEase(EaseBase ease)
        {
            if (!this.ease.GetType().Equals(ease.GetType()))
            {
                this.ease = ease;
            }
        }

        protected virtual void Update()
        {
            if(base.fillAmount != m_targetPos)
            {
                AnimateTimeCounter += Time.deltaTime;
                if(AnimateTimeCounter < AnimateTime)
                {
                    base.fillAmount = ease.GetEasedValue(m_startPos,m_targetPos,AnimateTimeCounter/AnimateTime);
                }
                else
                {
                    base.fillAmount = m_targetPos;
                    AnimateTimeCounter = 0;
                }
            }
        }

        public bool ForceComplete()
        {
            if (base.fillAmount != m_targetPos)
            {
                base.fillAmount = m_targetPos;
                AnimateTimeCounter = 0;
                return true;
            }
            return false;
        }
    }
}