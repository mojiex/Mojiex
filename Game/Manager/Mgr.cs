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
        public static Pool.PoolManager poolMgr;
        public static SaveManager saveMgr;
        public static void Init(Action onFinish = null)
        {
            uiMgr = new UIManager();
            poolMgr = new Pool.PoolManager();
            saveMgr = new SaveManager();

            mgrs.Add(uiMgr);
            mgrs.Add(poolMgr);
            mgrs.Add(saveMgr);

            foreach (var item in mgrs)
            {
                item.Init();
            }
            onFinish?.Invoke();
        }

        public static void Dispose()
        {
            foreach (var item in mgrs)
            {
                item.Dispose();
            }
            mgrs.Clear();
        }
    }

}