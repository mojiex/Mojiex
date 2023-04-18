#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2023/4/16
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;
using DG.Tweening;

namespace Mojiex
{
    public class AudioSystem
    {
        public static AudioSystem Inst
        {
            get
            {
                if (m_inst == null)
                {
                    m_inst = new AudioSystem();
                }
                return m_inst;
            }
        }

        private static AudioSystem m_inst;

        public enum AudioType
        {
            BGM,
            Sound,
        }
        #region Const
        public const string MASTER = "Master";
        public const float MAX_VOLUME = 20;
        public const float MIN_VOLUME = -80;
        #endregion

        #region Group
        private Transform m_root;
        private Dictionary<AudioType, Transform> m_group = new Dictionary<AudioType, Transform>();
        #endregion

        private AudioMixer m_mixer;
        private Dictionary<AudioType,Dictionary<string,AudioSource>> m_audioSourcesDic = new Dictionary<AudioType, Dictionary<string, AudioSource>>();
        private List<IAudio> m_registedAudioList = new List<IAudio>();
        #region Init

        private AudioSystem()
        {
            m_mixer = Resources.Load<AudioMixer>("AudioMixer");
            InitGroup();
            SetVolume(DataStatic.settings.GetMusicVolume(),AudioType.BGM);
            SetVolume(DataStatic.settings.GetSoundVolume(),AudioType.Sound);
            MDebug.Log("[AudioSystemInited:true]");
        }

        private void InitGroup()
        {
            m_root = new GameObject("[AudioSystemRoot]").transform;
            GameObject.DontDestroyOnLoad(m_root.gameObject);
            var types = Enum.GetValues(typeof(AudioType));
            for(int i = 0; i < types.Length; i++)
            {
                AudioType type = (AudioType)(types.GetValue(i));
                m_audioSourcesDic.Add(type, new Dictionary<string,AudioSource>());
                Transform group = new GameObject($"[{type}]").transform;
                group.SetParent(m_root);
                m_group.Add(type, group);
            }
        }
        #endregion

        public void Register(IAudio audio,AudioType type)
        {
            var groups = m_mixer.FindMatchingGroups($"{MASTER}/{type}");
            if (groups != null && groups.Length > 0)
            {
                m_registedAudioList.Add(audio);
                audio.GetAudioSource().outputAudioMixerGroup = groups[0];
            }
        }

        public void Play(string name,AudioType type)
        {
            if(type == AudioType.Sound && DataStatic.settings.IsSoundMute())
            {
                return;
            }else if(type == AudioType.BGM && DataStatic.settings.IsMusicMute())
            {
                return;
            }
            if (!m_audioSourcesDic[type].ContainsKey(name))
            {
                LoadAudio(name,type);
            }
            else
            {
                m_audioSourcesDic[type][name].Stop();
                m_audioSourcesDic[type][name].Play();
            }
        }

        public void SetMainVolume(float volume)
        {
            m_mixer.SetFloat(MASTER, TransVolume(volume));
        }

        public void SetVolume(float volume,AudioType type)
        {
            m_mixer.SetFloat(type.ToString(), TransVolume(volume));
        }

        public void Stop(string name,AudioType type)
        {
            if (m_audioSourcesDic[type].ContainsKey(name))
            {
                m_audioSourcesDic[type][name].Stop();
            }
        }

        public void LoadAudio(string name, AudioType type)
        {
            Addressables.LoadAssetAsync<AudioClip>(string.Format(Const.AUDIO, name)).Completed += (handle) =>
            {
                if(m_group[type].Find(name) != null || m_audioSourcesDic[type].ContainsKey(name))
                {
                    return;
                }
                GameObject audioGo = new GameObject(name);
                audioGo.transform.SetParent(m_group[type]);
                AudioSource audioSource = audioGo.AddComponent<AudioSource>();
                m_audioSourcesDic[type].Add(name, audioSource);
                var groups = m_mixer.FindMatchingGroups($"{MASTER}/{type}");
                audioSource.outputAudioMixerGroup = groups[0];
                audioSource.clip = handle.Result;
                audioSource.loop = type == AudioType.BGM;
                audioSource.Play();
            };
        }

        public static float TransVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            return volume * (MAX_VOLUME - MIN_VOLUME) + MIN_VOLUME;
        }
    }
}