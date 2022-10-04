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
using System.IO;

namespace Mojiex
{
    public class DictionaryStatic
    {
        public static DictionaryStatic Inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new DictionaryStatic();
                }
                return _inst;
            }
        }
        private static DictionaryStatic _inst;

        private ExcelDataHandler excelData;

        private DictionaryStatic()
        {
            excelData = InitExcelData();
        }

        public static ExcelDataHandler GetExcelData()
        {
            return DictionaryStatic.Inst.excelData;
        }

        private static ExcelDataHandler InitExcelData()
        {
            return PlayerPrefabHelper.GetObject<ExcelDataHandler>(ReadBinaryFile("ExcelDataHandler", ConstFilePath.ExcelAssetPath));
        }

        public static string ReadBinaryFile(string name, string path)
        {
            FileStream file = new FileStream(path + name, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(file);
            string res = reader.ReadString();
            reader.Close();
            file.Close();
            return res;
        }

        public List<ArchipelagoData> GetAllArchipelagoDatas() => excelData.m_ArchipelagoData;

        public List<LocalizationData> GetAllLocalizationDatas() => excelData.m_LocalizationData;
    }

}