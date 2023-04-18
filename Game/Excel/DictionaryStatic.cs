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
using UnityEngine.Networking;

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
        private string DictionaryData = "";
        public bool init = false;

        private ExcelDataHandler excelData;

        private DictionaryStatic()
        {
            //excelData = InitExcelData();
            SupportBehavior.Inst.StartCoroutine(GetExcelData("ExcelDataHandler", ConstFilePath.ExcelAssetPath));
        }

        public static ExcelDataHandler GetExcelData()
        {
            return DictionaryStatic.Inst.excelData;
        }

        private static ExcelDataHandler InitExcelData()
        {
            ReadBinaryFile("ExcelDataHandler", ConstFilePath.ExcelAssetPath);
            return PlayerPrefabHelper.GetObject<ExcelDataHandler>(DictionaryStatic.Inst.DictionaryData);
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

        public List<LocalizationData> GetAllLocalizationDatas()
        {
            return excelData.m_LocalizationData;
        }
        IEnumerator ReadData(string path)
        {
            UnityWebRequest www = UnityWebRequest.Get(path);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                throw new Exception("Net Connect fail");
            }
            using (BinaryReader reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                DictionaryStatic.Inst.DictionaryData = reader.ReadString();
            }
        }
        IEnumerator GetExcelData(string name, string path)
        {
            yield return SupportBehavior.Inst.StartCoroutine(ReadData(path + name));
            excelData = PlayerPrefabHelper.GetObject<ExcelDataHandler>(DictionaryStatic.Inst.DictionaryData);
            init = true;
        }
    }

}