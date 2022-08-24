using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/20
    public interface IMgr
    {
        void Init();
        bool IsInited();
        void Dispose();
    }
}