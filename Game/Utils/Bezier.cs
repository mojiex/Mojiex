#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/25
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public static class Bezier
    {

        /// <summary>
        /// 贝塞尔曲线
        /// 现支持小于3的inserts长度
        /// </summary>
        /// <returns></returns>
        public static Vector2 BezierHandler(Vector2 begin,Vector2 end,float t,params Vector2[] inserts)
        {
            int Length = inserts == null?0:inserts.Length;
            switch (Length)
            {
                case 0:
                    return Linear(begin,end,t);
                case 1:
                    return QuadraticBezier(begin,end,inserts[0],t);
                case 2:
                    return CubicBezier(begin,end,inserts[0],inserts[1],t);
                default: 
                    throw new ArgumentException("this version not support inserts length bigger than 2");
            }
        }
        /// <summary>
        /// 贝塞尔曲线
        /// 现支持小于3的inserts长度
        /// </summary>
        /// <returns></returns>
        public static Vector3 BezierHandler(Vector3 begin,Vector3 end,float t,params Vector3[] inserts)
        {
            int Length = inserts == null?0:inserts.Length;
            switch (Length)
            {
                case 0:
                    return Linear(begin,end,t);
                case 1:
                    return QuadraticBezier(begin,end,inserts[0],t);
                case 2:
                    return CubicBezier(begin,end,inserts[0],inserts[1],t);
                default: 
                    throw new ArgumentException("this version not support inserts length bigger than 2");
            }
        }

        /// <summary>
        /// Same as Mathf.Lerp();
        /// </summary>
        /// <param name="t"> value between 0-1 </param>
        /// <returns></returns>
	    public static Vector2 Linear(Vector2 begin,Vector2 end,float t)
        {
            t = Mathf.Clamp01(t);
            return begin + (end - begin) * t;
        }

        /// <summary>
        /// Same as Mathf.Lerp();
        /// </summary>
        /// <param name="t"> value between 0-1 </param>
        /// <returns></returns>
	    public static Vector3 Linear(Vector3 begin,Vector3 end,float t)
        {
            t = Mathf.Clamp01(t);
            return begin + (end - begin) * t;
        }

        /// <summary>
        /// 二次贝塞尔曲线
        /// insert_1:插值1
        /// </summary>
        public static Vector2 QuadraticBezier(Vector2 begin,Vector2 end,Vector2 insert_1,float t)
        {
            t = Mathf.Clamp01(t);
            float temp = 1-t;
            return temp*temp*begin + 2*t*temp*insert_1 + t*t*end;
        }
        /// <summary>
        /// 二次贝塞尔曲线
        /// insert_1:插值1
        /// </summary>
        public static Vector3 QuadraticBezier(Vector3 begin,Vector3 end,Vector3 insert_1,float t)
        {
            t = Mathf.Clamp01(t);
            float temp = 1-t;
            return temp*temp*begin + 2*t*temp*insert_1 + t*t*end;
        }

        /// <summary>
        /// 三次贝塞尔曲线
        /// insert_1:插值1
        /// insert_2:插值2
        /// </summary>
        public static Vector2 CubicBezier(Vector2 begin,Vector2 end,Vector2 insert_1,Vector2 insert_2,float t)
        {
            t = Mathf.Clamp01(t);
            float temp = 1-t;
            return temp*temp*temp*begin + 3*t*temp*temp*insert_1 + 3*t*t*temp*insert_2 + t*t*t*end;
        }
        /// <summary>
        /// 三次贝塞尔曲线
        /// insert_1:插值1
        /// insert_2:插值2
        /// </summary>
        public static Vector3 CubicBezier(Vector3 begin,Vector3 end,Vector3 insert_1,Vector3 insert_2,float t)
        {
            t = Mathf.Clamp01(t);
            float temp = 1-t;
            return temp*temp*temp*begin + 3*t*temp*temp*insert_1 + 3*t*t*temp*insert_2 + t*t*t*end;
        }

        ///
        /// n阶贝塞尔曲线，懒得写了，后续需要用到杨辉三角的内容去计算
        /// 
        // public static Vector2 MultiBezier(Vector2 begin,Vector2 end,Vector2[] inserts,float t)
        // {
        //     t = Mathf.Clamp01(t);
        //     float temp = 1-t;

        //     List<Vector2> insertList = new List<Vector2>(inserts);
        //     insertList.Insert(0,begin);
        //     insertList.Add(end);
        //     int Length = inserts.Length;

        //     Vector2 res = Vector2.zero;

        //     for (int i = 0; i < Length; i++)
        //     {
        //         float addTemp = 0;
        //         for (int j = 0; j < Length - 1; j++)
        //         {
        //             addTemp *= (j >= i ? temp : t);
        //         }
        //         float YangHuiTriangleValue;
        //         res += YangHuiTriangleValue * addTemp * insertList[i];
        //     }
        //     return res;
        // }
    }
}