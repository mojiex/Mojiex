using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

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
            while (queue.Count > 0)
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

        public static void IterateGameObject(this GameObject tf, System.Action<GameObject> action)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(tf.transform);
            while (queue.Count > 0)
            {
                var temp = queue.Dequeue();
                action(temp.gameObject);
                for (int i = 0; i < temp.childCount; i++)
                {
                    queue.Enqueue(temp.GetChild(i));
                }
            }
        }

        public static void SetState(this GameObject go,bool state)
        {
            if(go.activeSelf && !state)
            {
                go.SetActive(false);
            }
            else if((!go.activeSelf) && state)
            {
                go.SetActive(true);
            }
        }

        /// <summary>
        /// 获取parent到当前transform的路径，如果当前物体是目标parent返回空,如果没有找到parent返回根目录到当前对象的路径
        /// </summary>
        public static string GetPathToParentReverse(this Transform tf, Transform parent)
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

        /// <summary>
        /// 符合条件的字符不会被替换
        /// </summary>
        public static string RegexReplace(this string str, Regex regex, string replaceChar)
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

        public static RectTransform RectTransform(this Transform ts)
        {
            return ts as RectTransform;
        }

        public static RectTransform RectTransform(this GameObject go)
        {
            return go.GetComponent<RectTransform>();
        }

        public static T GetMissingComponent<T>(this GameObject com) where T : Component
        {
            if (com.TryGetComponent(out T component))
            {
                return component;
            }
            else
            {
                return com.AddComponent<T>();
            }
        }

        public static string[] SplitByComma(this string str)
        {
            return str.Split(',');
        }

        public static int[] ToInts(this string[] strings)
        {
            int[] res = new int[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                try
                {
                    res[i] = int.Parse(strings[i]);
                }
                catch (Exception e)
                {
                    MDebug.LogError(e);
                }
            }
            return res;
        }

        public static float[] ToFloats(this string[] strings)
        {
            float[] res = new float[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                try
                {
                    res[i] = float.Parse(strings[i]);
                }
                catch (Exception e)
                {
                    MDebug.LogError(e);
                }
            }
            return res;
        }

        public static bool Contain<T>(this T[] array, T checkElem)
        {
            if (array == null)
            {
                return false;
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(checkElem))
                {
                    return true;
                }
            }

            return false;
        }
        public static T[] FillData<T>(this T[] array, T data)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = data;
            }
            return array;
        }

        /// <summary>
        /// fill list with data,fill size equals list capacity
        /// </summary>
        public static List<T> FillData<T>(this List<T> array, T data)
        {
            for (int i = 0; i < array.Capacity; i++)
            {
                array[i] = data;
            }
            return array;
        }
        public static T[] ToArray<T>(this Mojiex.Math.Matrix<T> matrix)
        {
            T[] res = new T[matrix.Size];
            int k = 0;
            for (int i = 0; i < matrix.Row; i++)
            {
                for (int j = 0; j < matrix.Column; j++)
                {
                    res[k] = matrix[i, j];
                    k++;
                }
            }
            return res;
        }

        public static List<int> ToNonogramInfo(this bool[] matrix)
        {
            List<int> res = new List<int>();
            int count = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix[i])
                {
                    count++;
                    if (i + 1 >= matrix.Length)
                    {
                        res.Add(count);
                    }
                }
                else
                {
                    if (i >= 1 && matrix[i - 1])
                    {
                        res.Add(count);
                    }
                    count = 0;
                }
            }
            return res;
        }
        public static bool IsNullGameObject(this GameObject gameObject)
        {
            return GameObject.ReferenceEquals(gameObject, null);
        }

        public static int ToInt(this GameLevel value)
        {
            return (int)value;
        }

        /// <summary>
        /// min include, max exclude,
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<int> GetRandomInt(int min, int max, int count = 1)
        {
            List<int> res = new List<int>();
            if (max - min < count)
            {
                MDebug.LogError("your range is too small to get num");
                return res;
            }
            if (max - min == count)
            {
                for (int i = min; i < max; i++)
                {
                    res.Add(i);
                }
                return res;
            }

            if ((max - min) >= (2 * count))
            {
                int temp;
                while (res.Count < count)
                {
                    temp = UnityEngine.Random.Range(min, max);
                    if (!res.Contains(temp))
                    {
                        res.Add(temp);
                    }
                }
            }
            else
            {
                for (int i = min; i < max; i++)
                {
                    res.Add(i);
                }
                while (res.Count > count)
                {
                    res.RemoveAt(UnityEngine.Random.Range(0, res.Count));
                }
            }
            return res;
        }
    }
}