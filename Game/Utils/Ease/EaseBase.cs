#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/16
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex.Ease
{
    public class EaseBase
    {
        public virtual float GetEasedValue(float start, float end, float step)
        {
            throw new InvalidOperationException($"{GetType()} not implemented GetEasedValue");
        }

        public virtual Vector2 GetEasedValue(Vector2 start, Vector2 end, float step)
        {
            throw new InvalidOperationException($"{GetType()} not implemented GetEasedValue");
        }

        public virtual Vector3 GetEasedValue(Vector3 start, Vector3 end, float step)
        {
            throw new InvalidOperationException($"{GetType()} not implemented GetEasedValue");
        }

        public virtual Color GetEasedValue(Color start, Color end, float step)
        {
            throw new InvalidOperationException($"{GetType()} not implemented GetEasedValue");
        }
    }
}