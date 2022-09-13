#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/11
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Excel;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Text;

namespace Mojiex
{
    public class ExcelReader
    {
        private static Regex regex = new Regex(@"[^\[\] ]$");
        [MenuItem("Excel/ReadExcel")]
        public static void TestReflection()
        {
            string content = "";
            string name = "";
            List<string> ExcelPath = new List<string>();
            List<Type> TypeList = new List<Type>();
            DirectoryInfo floder = new DirectoryInfo(ConstFilePath.ExcelPath);
            var files = floder.GetFiles("*.xlsx");
            for (int i = 0; i < files.Length; i++)
            {
                CreateExcelDataScript(files[i],ref name);
                content += $"\tpublic List<{name}> m_{name} = new List<{name}>();{System.Environment.NewLine}";
                ExcelPath.Add(files[i].FullName);
                TypeList.Add(Type.GetType(name));
            }
            string FileTemplate =
                $"using System.Collections.Generic;{System.Environment.NewLine}" +
                $"using System;{System.Environment.NewLine}" +
                $"using UnityEngine;{System.Environment.NewLine}" +
                $"using Mojiex;{System.Environment.NewLine}" +
                $"{System.Environment.NewLine}" +
                $"[Serializable]{System.Environment.NewLine}" +
                $"public class ExcelDataHandler{System.Environment.NewLine}" +
                $"{{{System.Environment.NewLine}" +
                $"{content}"+
                $"}}";
            CreateTxtFile(FileTemplate, "ExcelDataHandler", ConstFilePath.ExcelScriptPath, true);

            ExcelDataHandler handler = new ExcelDataHandler();
            var fields = typeof(ExcelDataHandler).GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                IList list = fields[i].GetValue(handler) as IList;
                var datas = ReadDataExcel(ExcelPath[i], TypeList[i]);
                for (int j = 0; j < datas.Length; j++)
                {
                    list.Add(datas[j]);
                }
            }

            CreateBinaryFile(PlayerPrefabHelper.EncryptString(handler), "ExcelDataHandler", ConstFilePath.ExcelAssetPath);

            MDebug.Log("Excel Read Complete!");
            AssetDatabase.Refresh();
        }

        public static object[] ReadDataExcel(string filePath,Type type)
        {
            int columnNum = 0, rowNum = 0;
            DataRowCollection collection = ReadExcel(filePath, ref columnNum, ref rowNum);
            object[] values = new object[rowNum - 2];
            //第0行是备注，第1行是数据名和数据类型
            for (int i = 2; i < rowNum; i++)
            {
                if (IsEmptyRow(collection[i], columnNum))
                {
                    throw new System.MissingFieldException($"The row data has null,row index is {i}");
                }
                IExcelData data = Activator.CreateInstance(type) as IExcelData;
                data.FillData(collection[i]);
                values[i - 2] = (data);
            }
            return (values);
        }

        private static bool IsEmptyRow(DataRow collect,int column)
        {
            for (int i = 0; i < column; i++)
            {
                if (!collect.IsNull(i))
                    return false;
            }
            return true;
        }
        private static DataRowCollection ReadExcel(string filePath, ref int column,ref int row)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            stream.Close();

            DataSet res = excelReader.AsDataSet();
            column = res.Tables[0].Columns.Count;
            row = res.Tables[0].Rows.Count;
            return res.Tables[0].Rows;
        }

        private static void CreateExcelDataScript(FileInfo info,ref string name)
        {
            int columnNum = 0, rowNum = 0;
            string fileName = Path.GetFileNameWithoutExtension(info.Name);
            name = fileName;
            DataRowCollection collection = ReadExcel(info.FullName, ref columnNum, ref rowNum);
            string content = "", fill = "";
            for (int i = 0; i < columnNum; i++)
            {
                string[] str = Regex.Replace(collection[1][i].ToString(),@"\s","").Split(':');
                if(str.Length != 2)
                {
                    throw new ArgumentException($"Data must be style : variableName:variableType.Wrong row index : {i}");
                }
                ExcelDataType type = GetDataType(str[1]);
                content += $"\tpublic {str[1].ToLower()} {str[0]};{System.Environment.NewLine}";
                fill += $"\t\t{str[0]} = ";
                switch (type)
                {
                    case ExcelDataType.Int:
                        fill += $"int.Parse(data[{i}].ToString());";
                        break;
                    case ExcelDataType.IntArray:
                        fill += $"data[{i}].ToString().SplitByComma().ToInts();";
                        break;
                    case ExcelDataType.String:
                        fill += $"data[{i}].ToString();";
                        break;
                    case ExcelDataType.StringArray:
                        fill += $"data[{i}].ToString().SplitByComma();";
                        break;
                    case ExcelDataType.Float:
                        fill += $"float.Parse(data[{i}].ToString());";
                        break;
                    case ExcelDataType.FloatArray:
                        fill += $"data[{i}].ToString().SplitByComma().ToFloats();";
                        break;
                    default:
                        break;
                }
                fill += System.Environment.NewLine;
            }
            string FileTemplate =
                $"using System.Data;{System.Environment.NewLine}" +
                $"using System;{System.Environment.NewLine}" +
                $"using Mojiex;{System.Environment.NewLine}" +
                $"{System.Environment.NewLine}" +
                $"[Serializable]{System.Environment.NewLine}" +
                $"public class {fileName} : IExcelData{System.Environment.NewLine}" +
                $"{{{System.Environment.NewLine}" +
                $"{content}" +
                $"\tpublic void FillData(DataRow data){System.Environment.NewLine}" +
                $"\t{{{System.Environment.NewLine}" +
                $"{fill}" +
                $"\t}}{System.Environment.NewLine}" +
                $"}}";
            CreateTxtFile(FileTemplate, fileName, ConstFilePath.ExcelScriptPath, true);
        }

        private static ExcelDataType GetDataType(string value)
        {
            bool isArray = value.Contains("[]");
            string typeStr = value.RegexReplace(regex, "").ToLower();
            if (typeStr.Equals(ExcelDataType.Int.ToString().ToLower()))
            {
                return isArray ? ExcelDataType.IntArray : ExcelDataType.Int;
            }
            if (typeStr.Equals(ExcelDataType.Float.ToString().ToLower()))
            {
                return isArray ? ExcelDataType.FloatArray : ExcelDataType.Float;
            }
            if (typeStr.Equals(ExcelDataType.String.ToString().ToLower()))
            {
                return isArray ? ExcelDataType.StringArray : ExcelDataType.String;
            }
            throw new ArgumentException($"{value} is not an available type");
        }

        private static void CreateTxtFile(string content, string name, string path, bool rewrite = false , string rear = ".cs")
        {
            //获取当前所选择的目录（相对于Assets的路径）
            string newFileName = name + rear;
            //如果文件路径已经被占用，则变成临时文件路径
            string newFilePath = path + "/" + newFileName;

            if ((!rewrite) && File.Exists(newFilePath))
            {
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //File.WriteAllText(newFilePath, content);
            bool encoderShouldEmitUTF8Identifier = true; //参数指定是否提供 Unicode 字节顺序标记
            bool throwOnInvalidBytes = false;//是否在检测到无效的编码时引发异常
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
            bool append = false;
            //写入文件
            StreamWriter streamWriter = new StreamWriter(newFilePath, append, encoding);
            streamWriter.Write(content);
            streamWriter.Close();
            AssetDatabase.ImportAsset(newFilePath);
            AssetDatabase.Refresh();
        }

        private static void CreateBinaryFile(string content, string name, string path)
        {
            FileStream file = new FileStream(path + name, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter binaryWriter = new BinaryWriter(file);
            binaryWriter.Write(content);
            binaryWriter.Flush();
            binaryWriter.Close();
            file.Close();
        }
    }
}