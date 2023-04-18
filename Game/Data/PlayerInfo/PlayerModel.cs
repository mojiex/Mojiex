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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace Mojiex
{
    public class PlayerModel
    {
        private PlayerInfo info;

        public PlayerModel(PlayerInfo info)
        {
            this.info = info;
        }

        public void Save()
        {
            Mgr.saveMgr.Save(info);
        }
    }
}