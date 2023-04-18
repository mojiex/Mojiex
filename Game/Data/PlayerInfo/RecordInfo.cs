#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2023/3/8
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class RecordInfo : BaseSaveInfo
    {
        public long timeStamp;
        public long enterTimeStamp;
        public override void InitDefault()
        {
            timeStamp = 0;
            enterTimeStamp = 0;
        }
    }
}