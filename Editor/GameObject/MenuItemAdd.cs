using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;
using System;
using System.Text;
using UnityEngine.UI;
using LitJson;


namespace Mojiex
{
	//CreateTime : 2022/8/20
	public class MenuItemAdd : MonoBehaviour
	{
		/// <summary>
		/// 正则表达式，匹配数字、字母下划线
		/// </summary>
		private static Regex regex = new Regex(@"[a-zA-Z_0-9]$");
		private static string filePath = Application.dataPath + "/_Script/UI/";
		private static Type[] tryTypes = new Type[] {typeof(Button),typeof(Toggle),typeof(Text),
			typeof(Image),typeof(ScrollRect),typeof(LayoutGroup), typeof(Slider),typeof(Canvas),typeof(Transform) }; 

		[MenuItem("GameObject/MUITool/CreateUIScript", priority = 0)]
		private static void MyTest(MenuCommand menuCommand)
		{
			GameObject select = menuCommand.context as GameObject;
			CreateTxtFile(FormatComTemplate(select.transform, "Assets/Mojiex/Editor/GameObject/UIPanelComTemplate.cs.txt"), select.name + "Com", true);
			CreateTxtFile(FormatTemplate(select.transform, "Assets/Mojiex/Editor/GameObject/UIPanelTemplate.cs.txt"), select.name);
		}
        
		//取得要创建文件的路径
		public static string GetSelectPathOrFallback()
		{
			string path = "Assets";
			//遍历选中的资源以获得路径
			//Selection.GetFiltered是过滤选择文件或文件夹下的物体，assets表示只返回选择对象本身
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);
				if (!string.IsNullOrEmpty(path) && File.Exists(path))
				{
					path = Path.GetDirectoryName(path);
					break;
				}
			}
			return path;
		}

		/// <summary>
		/// 传入Transform和模板路径，输出格式化后的文本（不是文件）
		/// </summary>
		private static string FormatComTemplate(Transform trans,string templateFilePath)
		{
			StreamReader streamReader = new StreamReader(templateFilePath);
			string text = streamReader.ReadToEnd();
			streamReader.Close();

			string Component = "";
			List<string> namePool = new List<string>();
			string Getcomponent = "";
			var transList = trans.GetAllChildTransform();

			for (int i = 0; i < transList.Length; i++)
			{
				int j = 0;
				Component item;
				while (!transList[i].TryGetComponent(tryTypes[j] , out item))
				{
					j++;
				}
				//防止重名
				string comName = item.name;
				string tempName = comName;
				tempName = tempName.RegexReplace(regex, "");
				int tempTimes = 1;
				while (namePool.Contains(tempName))
				{
					tempName = comName + $"_{tempTimes}";
					tempTimes++;
				}
				namePool.Add(tempName);

				if (transList[i].Equals(trans))
				{
					Component += $"\tpublic {tryTypes[j]} {tempName};\n";
					Getcomponent += $"\t\t{tempName} = GetComponent<{item.GetType()}>();\n";
				}
				else
				{
					Component += $"\tpublic {tryTypes[j]} {tempName};\n";
					Getcomponent += $"\t\t{tempName} = transform.Find(\"{transList[i].GetPathToParentReverse(trans)}\").GetComponent<{tryTypes[j]}>();\n";
				}
			}
			text = Regex.Replace(text, "#PANELNAMECOM#", trans.name + "Com");
			text = Regex.Replace(text, "#COMPONENT#", Component);
			text = Regex.Replace(text, "#GETCOMPONENT#", Getcomponent);
			text = Regex.Replace(text, "#CREATETIME#", System.DateTime.Now.ToString("d"));
			
			return text;
		}

		private static string FormatTemplate(Transform trans,string templateFilePath)
		{
			StreamReader streamReader = new StreamReader(templateFilePath);
			string text = streamReader.ReadToEnd();
			streamReader.Close();

			text = Regex.Replace(text, "#PANELNAMECOM#", trans.name + "Com");
			text = Regex.Replace(text, "#CREATETIME#", System.DateTime.Now.ToString("d"));
			text = Regex.Replace(text, "#PANELNAME#", trans.name);

			return text;
		}
		private static void CreateTxtFile(string content, string name,bool rewrite = false)
		{
			string path = filePath;
			//获取当前所选择的目录（相对于Assets的路径）
			string newFileName = name + ".cs";
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
	}

	class CreatePanelScriptAsset : EndNameEditAction
	{
		public override void Action(int instanceId, string pathName, string resourceFile)
		{
			//创建资源
			UnityEngine.Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
			ProjectWindowUtil.ShowCreatedAsset(obj);//高亮显示资源
		}

		internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
		{
			//获取要创建资源的绝对路径
			string fullPath = Path.GetFullPath(pathName);
			//读取本地的模板文件
			StreamReader streamReader = new StreamReader(resourceFile);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			//获取文件名，不含扩展名
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);

			//将模板类中的类名替换成你创建的文件名
			text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithoutExtension);
			text = Regex.Replace(text, "#CREATETIME#", System.DateTime.Now.ToString("d"));
			bool encoderShouldEmitUTF8Identifier = true; //参数指定是否提供 Unicode 字节顺序标记
			bool throwOnInvalidBytes = false;//是否在检测到无效的编码时引发异常
			UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
			bool append = false;
			//写入文件
			StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
			streamWriter.Write(text);
			streamWriter.Close();
			//刷新资源管理器
			AssetDatabase.ImportAsset(pathName);
			AssetDatabase.Refresh();
			return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
		}
	}
}