using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/27
    public class DataStatic
    {
        public static PlayerModel player;
        public static StatisticsInfo statistics;
        public static RecordModel record;
        public static SettingsModel settings;

        public static void Init()
        {
            SupportBehavior.Inst.AddGameQuitMethod(Save);
            SupportBehavior.Inst.AddFocusMethod(Save);
            player = new PlayerModel(Mgr.saveMgr.Load<PlayerInfo>());
            record = new RecordModel(Mgr.saveMgr.Load<RecordInfo>());
            settings = new SettingsModel(Mgr.saveMgr.Load<SettingsInfo>());
            statistics = Mgr.saveMgr.Load<StatisticsInfo>();
            //MDebug.Log(player.GetInfo().name);
        }

        public static void Save(bool state)
        {
            Save();
        }
        public static void Save()
        {
            player.Save();
            record.Save();
            settings.Save();
            Mgr.saveMgr.Save(statistics);
        }
    }
}