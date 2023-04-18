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
    public class RecordModel
    {
        private RecordInfo m_info;

        public RecordModel(RecordInfo info)
        {
            this.m_info = info;
            SupportBehavior.Inst.AddGameQuitMethod(SetExitTimeStamp);
        }

        public DateTime GetLastExitDateTime()
        {
            return TimeUtils.GetTime(GetExitTimeStamp());
        }

        public DateTime GetLastEnterDateTime()
        {
            return TimeUtils.GetTime(GetEnterTimeStamp());
        }

        public bool IsNewDay()
        {
            return GetLastEnterDateTime().Date != TimeUtils.Now().Date || GetLastExitDateTime().Date != TimeUtils.Now().Date;
        }
        public long GetExitTimeStamp()
        {
            return m_info.timeStamp;
        }
        public void SetExitTimeStamp(long value)
        {
            m_info.timeStamp = value;
            Save();
        }

        public long GetEnterTimeStamp()
        {
            return m_info.enterTimeStamp;
        }
        public void SetEnterTimeStamp(long value)
        {
            m_info.enterTimeStamp = value;
            Save();
        }

        public void SetExitTimeStamp()
        {
            SetExitTimeStamp(TimeUtils.GetTimeStamp());
        }

        public void SetEnterTimeStamp()
        {
            SetEnterTimeStamp(TimeUtils.GetTimeStamp());
        }

        public void Save()
        {
            Mgr.saveMgr.Save(m_info);
        }
    }
}