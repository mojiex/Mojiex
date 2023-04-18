#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/8/28
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    /// <summary>
    /// 用于整个游戏中都可能触发的事件
    /// The events which used of all game life
    /// </summary>
    public enum GameEvents
    {
        #region DATA
        SaveData,//需要保存游戏数据的时候触发
        #endregion

        #region UI
        OnUpdateUI,
        #endregion

        #region GameLogic
        
        #endregion
    }
}