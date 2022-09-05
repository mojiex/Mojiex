using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/20
    public class EventSystem<T> where T: Enum
    {
        private static Dictionary<T, List<Action<object[]>>> EventsActionMap = new Dictionary<T, List<Action<object[]>>>();

        ~EventSystem()
        {
            RemoveAllListener();
        }

        public static void Trigger(T key,params object[] objs)
        {
            if (EventsActionMap.ContainsKey(key))
            {
                var list = EventsActionMap[key];
                Debug.Log(list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        list[i].Invoke(objs);
                    }
                    catch (Exception e)
                    {
                        MDebug.LogError(e);
                    }
                }
            }
        }

        public static void AddListener(T key, Action<object[]> CallBack)
        {
            if (EventsActionMap.ContainsKey(key))
            {
                EventsActionMap[key].Add(CallBack);
            }
            else
            {
                List<Action<object[]>> list = new List<Action<object[]>>() { CallBack };
                EventsActionMap.Add(key, list);
            }
        }

        public static void RemoveListener(T key, Action<object[]> CallBack)
        {
            if (EventsActionMap.ContainsKey(key) && EventsActionMap[key].Contains(CallBack))
            {
                EventsActionMap[key].Remove(CallBack);
            }
        }

        public static void RemoveAllListener()
        {
            foreach (var item in EventsActionMap)
            {
                item.Value.Clear();
                EventsActionMap.Remove(item.Key);
            }
        }

        public static void RemoveAllListener(T key)
        {
            if (EventsActionMap.ContainsKey(key))
            {
                EventsActionMap[key].Clear();
                EventsActionMap.Remove(key);
            }
        }
    }
}