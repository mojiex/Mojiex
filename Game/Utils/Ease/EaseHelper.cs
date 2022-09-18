#region Header
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
    public class EaseHelper
    {
	    public static EaseBase GetEase(EaseType easeType)
        {
            return Activator.CreateInstance(Type.GetType(easeType.ToString())) as EaseBase;
        }
    }
}