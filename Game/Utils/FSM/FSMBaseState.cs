#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/10/4
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class FSMBaseState
    {
        public int priority { get; private set; } = 0;

        public void SetPriority(int value) => priority = value;
        public virtual void Enter() { }
        public virtual void Execute() { }
        public virtual void Exit() { }
    }
}