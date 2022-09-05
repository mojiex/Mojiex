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

        public static void Init()
        {
            player = new PlayerModel();
            player.Init();
            //MDebug.Log(player.GetInfo().name);
        }
    }
}