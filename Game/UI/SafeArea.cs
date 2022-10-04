#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/10/2
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Mojiex
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private RectTransform rectTransform;

        private Rect lastRect = new Rect();
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            RefreshSafeArea();
        }

        private void FixedUpdate()
        {
            RefreshSafeArea();
        }
        private void RefreshSafeArea()
        {
            Rect rect = Screen.safeArea;
            if (lastRect.Equals(rect))
                return;

            lastRect = rect;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.pivot = Vector2.zero;
            rectTransform.anchoredPosition = new Vector2(rect.xMin, rect.yMin);
            Vector2 size = new Vector2(1080f, (1080.0f / rect.width) * rect.height);
            rectTransform.sizeDelta = size;
            //rectTransform.anchorMin = Vector2.one * 0.5f;
            //rectTransform.anchorMax = Vector2.one * 0.5f;
            MDebug.Log($"{rect.xMin},{rect.yMin},{rect.width},{rect.height}");
        }
    }
}