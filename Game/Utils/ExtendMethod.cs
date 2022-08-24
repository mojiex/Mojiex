using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

namespace Mojiex
{
    //CreateTime : 2022/8/23
    public static class ExtendMethod
    {
        /// <summary>
        /// 获取物体所有子对象，包括自己和子对象的子对象
        /// </summary>
	    public static Transform[] GetAllChildTransform(this Transform tf)
        {
            Queue<Transform> queue = new Queue<Transform>();
            List<Transform> transList = new List<Transform>();
            queue.Enqueue(tf);
            while(queue.Count > 0)
            {
                var temp = queue.Dequeue();
                transList.Add(temp);
                for (int i = 0; i < temp.childCount; i++)
                {
                    queue.Enqueue(temp.GetChild(i));
                }
            }
            return transList.ToArray();
        }

        /// <summary>
        /// 获取parent到当前transform的路径，如果当前物体是目标parent返回空,如果没有找到parent返回根目录到当前对象的路径
        /// </summary>
        public static string GetPathToParentReverse(this Transform tf,Transform parent)
        {
            string path = "";
            //直接改动tf不知道会不会出问题，用temp当中间变量
            var temp = tf;
            List<Transform> parentList = new List<Transform>();
            while ((!temp.Equals(parent))
                && temp.parent != null)
            {
                parentList.Add(temp);
                temp = temp.parent;
            }

            for (int i = parentList.Count - 1; i >= 0; i--)
            {
                path += parentList[i].name + "/";
            }
            return path;
        }

        public static string RegexReplace(this string str,Regex regex,string replaceChar)
        {
            string res = "";
            foreach (var item in str)
            {
                
                if (!regex.IsMatch(item.ToString()))
                {
                    res += replaceChar;
                }
                else
                {
                    res += item;
                }
            }
            return res;
        }
    }
}