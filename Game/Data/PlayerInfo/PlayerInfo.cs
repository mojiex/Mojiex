#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/8/27
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class PlayerInfo:BaseSaveInfo
    {
        public int level;
        public int Gem;
        public List<int> characterList;
        public int star;
        public string name;
        public override void InitDefault()
        {
            level = 0;
            Gem = 0;
            star = 6;
            name = "Mojiex";
            characterList = new List<int> { 11, 2, 654, 321, 222, 3 };
        }
    }
}