#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2023/4/16
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class SettingsInfo : BaseSaveInfo
    {
        public float Music;
        public float Sound;
        public bool Vibrate;
        public override void InitDefault()
        {
            Music = 0.8f;
            Sound = 0.8f;
            Vibrate = true;
        }
    }
}