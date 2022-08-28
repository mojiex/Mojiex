using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/25
    public class UIGraphicRaycaster:GraphicRaycaster
    {
        public void SetLayerMask(LayerMask m)
        {
            m_BlockingMask = m;
        }
    }
}