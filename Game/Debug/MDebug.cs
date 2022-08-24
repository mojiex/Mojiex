using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/20
    public class MDebug
    {
        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }
    }

}