#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2023/3/8
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class TimeUtils
    {
        public static DateTime Now()
        {
            return DateTimeOffset.Now.DateTime;
        }

        public static DateTime GetNextDay()
        {
            return GetDays(1);
        }

        public static DateTime GetDays(double days)
        {
            return DateTimeOffset.Now.AddDays(days).Date;
        }
        public static TimeSpan GetTodayLeft()
        {
            return GetNextDay() - Now();
        }

        public static long GetTimeStamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public static DateTime GetTime(long timeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timeStamp).LocalDateTime;
        }

        #region String
        public static string GetTimeString(TimeCounter timer)
        {
            return $"{(((int)timer.CurrentTime) / 60).ToString().PadLeft(2,'0')}:{(((int)timer.CurrentTime) % 60).ToString().PadLeft(2, '0')}";
        }
        #endregion
    }
}