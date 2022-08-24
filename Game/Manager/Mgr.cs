using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/21
    public class Mgr
    {
        private static List<IMgr> mgrs = new List<IMgr>();

        public static UIManager uiMgr;
        public static void Init(Action onFinish = null)
        {
            uiMgr = new UIManager();
            mgrs.Add(uiMgr);

            foreach (var item in mgrs)
            {
                item.Init();
            }
            onFinish?.Invoke();
        }
    }

}