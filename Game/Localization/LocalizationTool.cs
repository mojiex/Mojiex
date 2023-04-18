#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/21
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Mojiex
{
    public class LocalizationTool
    {
        public static LocalizationTool Inst { get; } = new LocalizationTool();

        //默认语言是开发时使用的语言，根据实际情况进行替换
        private const SystemLanguage DEFAULT_LANG = SystemLanguage.English;
        public SystemLanguage CurrentLanguage { get; set; } = SystemLanguage.Chinese;
        /// <summary>
        /// (language,(key,language text))
        /// </summary>
        private Dictionary<SystemLanguage, Dictionary<string, string>> LocalizationDataDic = new Dictionary<SystemLanguage, Dictionary<string, string>>();
        private LocalizationTool() { }

        public async void InitLangData(Action onFinish)
        {
            for (int j = 0; j < 100; j++)
            {
                MDebug.Log($"[DictionaryStatic : Inited {DictionaryStatic.Inst.init}]");
                if (DictionaryStatic.Inst.init)
                {
                    var datas = DictionaryStatic.Inst.GetAllLocalizationDatas();
                    var langDataType = typeof(LocalizationData);
                    //var LangPool = from lang in langDataType select lang.Name;
                    Array LangEnum = Enum.GetValues(typeof(SystemLanguage));
                    foreach (var item in LangEnum)
                    {
                        System.Reflection.FieldInfo field = langDataType.GetField(item.ToString());
                        if (field != null)
                        {
                            Dictionary<string, string> keyLangDic = new Dictionary<string, string>();
                            for (int i = 0; i < datas.Count; i++)
                            {
                                if (keyLangDic.ContainsKey(datas[i].Key))
                                {
                                    throw new ArgumentException($"key repeated,key is {datas[i].Key},index:{i}");
                                }
                                keyLangDic.Add(datas[i].Key, field.GetValue(datas[i]).ToString());
                            }
                            LocalizationDataDic.Add((SystemLanguage)item, keyLangDic);
                        }
                    }
                    onFinish?.Invoke();
                    break;
                }
                else
                {
                    await System.Threading.Tasks.Task.Delay(100);
                }
            }

        }

        public string GetLocalizationTxt(string key)
        {
            string res = GetLocalizationTxt(CurrentLanguage, key);
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            res = GetLocalizationTxt(DEFAULT_LANG, key);
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            throw new ArgumentNullException("Default Lnguage Data not exist,Check your data file");
        }

        public string GetLocalizationTxt(SystemLanguage language, string key)
        {
            if (LocalizationDataDic.ContainsKey(language)
                 && LocalizationDataDic[language].ContainsKey(key)
                 && !string.IsNullOrEmpty(LocalizationDataDic[language][key]))
            {
                return LocalizationDataDic[language][key];
            }
            MDebug.LogWarning($"Language Key Not Found,language : {language},Key : {key}");
            return "";
        }
    }
}
