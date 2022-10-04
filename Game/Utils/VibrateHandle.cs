#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/24
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class VibrateHandle
    {
        private static Coroutine currentVibrate;
        public static void LightVibrate() => Vibrate(1);
        public static void HeaveVibrate() => Vibrate(2);
        public static void Vibrate(int times)
        {
            if(currentVibrate != null)
            {
                SupportBehavior.Inst.StopCoroutine(currentVibrate);
            }
            currentVibrate = SupportBehavior.Inst.StartCoroutine(VibrateAction(times));
        }
        private static IEnumerator VibrateAction(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Handheld.Vibrate();
                //0.5s是Handheld.Vibrate()单次震动时长
                yield return new WaitForSeconds(0.5f);
            }
            currentVibrate = null;
        }
    }
}