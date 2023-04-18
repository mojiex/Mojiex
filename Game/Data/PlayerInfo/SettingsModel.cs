using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mojiex
{
    public class SettingsModel
    {
        private SettingsInfo m_info;
        public SettingsModel(SettingsInfo info)
        {
            m_info = info;
        }

        public void SetMusicVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            m_info.Music = volume;
        }
        public float GetMusicVolume()
        {
            return m_info.Music;
        }
        public void SetSoundVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            m_info.Sound = volume;
        }
        public float GetSoundVolume()
        {
            return m_info.Sound;
        }

        public void SetVibrate(bool vibrate)
        {
            m_info.Vibrate = vibrate;
        }
        public bool GetVibrate()
        {
            return m_info.Vibrate;
        }
        public bool IsMusicMute()
        {
            return m_info.Music == 0;
        }
        public bool IsSoundMute()
        {
            return m_info.Sound == 0;
        }
        public void Save()
        {
            Mgr.saveMgr.Save(m_info);
        }
    }

}
