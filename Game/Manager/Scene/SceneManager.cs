#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/9/17
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using U = UnityEngine.SceneManagement;

namespace Mojiex
{
    public class SceneManager : IMgr
    {

        public void Dispose()
        {
            
        }

        public void Init()
        {
            
        }

        public bool IsInited()
        {
            return true;
        }

        /// <summary>
        /// 加载场景统一用异步加载
        /// </summary>
        public void LoadScene(string SceneName,Action<float> Processor = null ,Action FinishCB = null)
        {
            void sceneLoaded(U.Scene a, U.LoadSceneMode b)
            {
                FinishCB?.Invoke();
                U.SceneManager.sceneLoaded -= sceneLoaded;
            }

            U.SceneManager.sceneLoaded += sceneLoaded;
            AsyncOperation operation = U.SceneManager.LoadSceneAsync(SceneName);
            if(Processor != null)
            {
                SupportBehavior.Inst.StartCoroutine(Load(operation, Processor));
            }
        }

        /// <summary>
        /// 加载场景统一用异步加载
        /// </summary>
        public void LoadScene(int SceneIndex, Action<float> Processor = null, Action FinishCB = null)
        {
            void sceneLoaded(U.Scene a, U.LoadSceneMode b)
            {
                FinishCB?.Invoke();
                U.SceneManager.sceneLoaded -= sceneLoaded;
            }

            U.SceneManager.sceneLoaded += sceneLoaded;
            AsyncOperation operation = U.SceneManager.LoadSceneAsync(SceneIndex);
            if (Processor != null)
            {
                SupportBehavior.Inst.StartCoroutine(Load(operation, Processor));
            }
        }

        private IEnumerator Load(AsyncOperation op, Action<float> Processor)
        {
            op.allowSceneActivation = false;
            while (!op.isDone && op.progress < 0.9f)
            {
                Processor?.Invoke(op.progress);
                yield return new WaitForEndOfFrame();
            }
            Processor?.Invoke(1.0f);
            while (true)
            {
                if (Input.anyKey)
                {
                    break;
                }
                yield return null;
            }
            MDebug.Log("Load Successful");
            op.allowSceneActivation = true;
        }
    }
}