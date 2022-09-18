﻿#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/17
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex.Ease
{
    public class SineOut:EaseBase
    {
        public override Color GetEasedValue(Color start, Color end, float step)
        {
            return (end - start) * Mathf.Sin(Mathf.Clamp01(step) * (Mathf.PI / 2)) + start;
        }

        public override float GetEasedValue(float start, float end, float step)
        {
            return (end - start) * Mathf.Sin(Mathf.Clamp01(step) * (Mathf.PI / 2)) + start;
        }

        public override Vector2 GetEasedValue(Vector2 start, Vector2 end, float step)
        {
            return (end - start) * Mathf.Sin(Mathf.Clamp01(step) * (Mathf.PI / 2)) + start;
        }

        public override Vector3 GetEasedValue(Vector3 start, Vector3 end, float step)
        {
            return (end - start) * Mathf.Sin(Mathf.Clamp01(step) * (Mathf.PI / 2)) + start;
        }
    }
}