#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/8/27
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class SaveManager : IMgr
    {
        private bool isInited = false;

        public void Dispose()
        {
            DataStatic.Save();
        }

        public void Init()
        {
            DataStatic.Init();
            isInited = true;
        }

        public T Load<T>() where T : BaseSaveInfo, new()
        {
            return PlayerPrefabHelper.Load<T>();
        }

        public void Save<T>(T data) where T : BaseSaveInfo
        {
            PlayerPrefabHelper.Save(data);
        }
        public bool IsInited()
        {
            return isInited;
        }
    }
}