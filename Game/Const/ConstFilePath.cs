#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/12
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class ConstFilePath
    {
        #region UI
        public static string panelFilePath = Application.dataPath + "/_Script/UI/";
        public static string ItemFilePath = Application.dataPath + "/_Script/UI/Item";
        #endregion

        #region Excel
        public static string ExcelPath = Application.dataPath + "/Mojiex/Excel/File";
        public static string ExcelScriptPath = Application.dataPath + "/Mojiex/Excel/Script";
        public static string ExcelAssetPath = Application.dataPath + "/Mojiex/Excel/Asset/";
        #endregion
    }
}