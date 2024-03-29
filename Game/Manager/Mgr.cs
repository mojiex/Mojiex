﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/21
    public class Mgr
    {
        ~Mgr()
        {
            Dispose();
        }
        private static List<IMgr> mgrs = new List<IMgr>();

        public static UIManager uiMgr;
        public static Pool.PoolManager poolMgr;
        public static SaveManager saveMgr;
        public static SceneManager sceneMgr;
        public static void Init(Action onFinish = null)
        {
            uiMgr = new UIManager();
            poolMgr = new Pool.PoolManager();
            saveMgr = new SaveManager();
            sceneMgr = new SceneManager();

            mgrs.Add(uiMgr);
            mgrs.Add(poolMgr);
            mgrs.Add(saveMgr);
            mgrs.Add(sceneMgr);

            foreach (var item in mgrs)
            {
                item.Init();
            }

            LocalizationTool.Inst.InitLangData(onFinish);
            DataStatic.record.SetEnterTimeStamp();
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