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
    //该接口用于model类，model类用于数据处理，和BaseSaveInfo共同承担数据处理部分
    public interface IDataModel
    {
        /// <summary>
        /// 初始化中需要Load数据
        /// </summary>
        void Init();
        void Save();
    }
}