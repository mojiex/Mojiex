using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/21
    public class SupportBehavior : MonoBehaviour
    {
        private class AfterAction
        {
            public Action action;
            public float time;
            public bool isRealtime;
        }
        public static SupportBehavior Inst
        {
            get
            {
                if (_inst == null)
                {
                    GameObject supportGameObject = new GameObject("SupportBehavior");
                    _inst = supportGameObject.AddComponent<SupportBehavior>();
                    DontDestroyOnLoad(supportGameObject);
                    _inst.afterActions = new List<AfterAction>();
                    _inst.AddUpdateMethod(_inst.UpdateAfterAction);
                }
                return _inst;
            }
        }
        private static SupportBehavior _inst;

        private Action onUpdate;
        private Action onLateUpdate;
        private Action onFixedUpdate;
        private Action onQuitGame;

        private List<float> TimeCounter;
        private List<Action> AfterCallBack;
        private List<AfterAction> afterActions;

        /// <summary>
        /// time时间后执行action，执行一次
        /// </summary>
        public void AddAfterAction(float time, Action action,bool isRealtime = false)
        {
            afterActions.Add(new AfterAction()
            {
                time = time,
                action = action,
                isRealtime = isRealtime,
            });
        }
        public void AddUpdateMethod(Action onUpdate,bool allowReapeat = false)
        {
            if(!allowReapeat && this.onUpdate != null&&(this.onUpdate.GetInvocationList().Contain(onUpdate)))
            {
                MDebug.Log("Action Repeat");
                return;
            }
            this.onUpdate += onUpdate;
        }
        public void RemoveUpdateMethod(Action onUpdate)
        {
            this.onUpdate -= onUpdate;
        }
        public void AddLateUpdateMethod(Action onUpdate,bool allowReapeat = false)
        {
            if (!allowReapeat && onLateUpdate != null && onLateUpdate.GetInvocationList().Contain(onUpdate))
            {
                MDebug.Log("Action Repeat");
                return;
            }
            this.onLateUpdate += onUpdate;
        }
        public void RemoveLateUpdateMethod(Action onUpdate)
        {
            this.onLateUpdate -= onUpdate;
        }

        public void AddFixedUpdateMethod(Action onUpdate, bool allowReapeat = false)
        {
            if (!allowReapeat && onFixedUpdate != null && onFixedUpdate.GetInvocationList().Contain(onUpdate))
            {
                MDebug.Log("Action Repeat");
                return;
            }
            this.onFixedUpdate += onUpdate;
        }
        public void RemoveFixedUpdateMethod(Action onUpdate)
        {
            this.onFixedUpdate -= onUpdate;
        }
        public void AddGameQuitMethod(Action gameQuit)
        {
            onQuitGame += gameQuit;
        }
        public void RemoveGameQuitMethod(Action gameQuit)
        {
            onQuitGame -= gameQuit;
        }

        private void Update()
        {
            onUpdate?.Invoke();
        }
        private void LateUpdate()
        {
            onLateUpdate?.Invoke();
        }
        private void FixedUpdate()
        {
            onFixedUpdate?.Invoke();
        }
        private void OnDestroy()
        {
            //TimeCounter.Clear();
            //AfterCallBack.Clear();
            afterActions.Clear();
        }

        private void OnApplicationQuit()
        {
            onQuitGame?.Invoke();
        }

        private void OnApplicationFocus(bool focus)
        {
            
        }
        private void UpdateAfterAction()
        {
            //由于有删除操作，使用由大向小遍历，保证下标存在
            for (int i = afterActions.Count - 1; i >= 0; i--)
            {
                afterActions[i].time -= afterActions[i].isRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
                if (afterActions[i].time <= 0)
                {
                    afterActions[i].action?.Invoke();
                    afterActions.RemoveAt(i);
                }
            }
        }

        private void Reset()
        {
            onUpdate = UpdateAfterAction;
            onLateUpdate = null;
            onFixedUpdate = null;

            afterActions.Clear();
        }
    }

}