using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    //CreateTime : 2022/8/21
    public class SupportBehavior : MonoBehaviour
    {
        public static SupportBehavior Inst
        {
            get
            {
                if (_inst == null)
                {
                    GameObject supportGameObject = new GameObject("SupportBehavior");
                    _inst = supportGameObject.AddComponent<SupportBehavior>();
                    DontDestroyOnLoad(supportGameObject);
                    _inst.TimeCounter = new List<float>();
                    _inst.AfterCallBack = new List<Action>();
                    _inst.AddUpdateMethod(_inst.UpdateAfterAction);
                }
                return _inst;
            }
        }
        private static SupportBehavior _inst;

        private Action onUpdate;
        private Action onLateUpdate;
        private Action onFixedUpdate;

        private List<float> TimeCounter;
        private List<Action> AfterCallBack;

        /// <summary>
        /// time时间后执行action，执行一次
        /// </summary>
        public void AddAfterAction(float time, Action action)
        {
            TimeCounter.Add(time);
            AfterCallBack.Add(action);
        }
        public void AddUpdateMethod(Action onUpdate)
        {
            this.onUpdate += onUpdate;
        }
        public void RemoveUpdateMethod(Action onUpdate)
        {
            this.onUpdate -= onUpdate;
        }
        public void AddLateUpdateMethod(Action onUpdate)
        {
            this.onLateUpdate += onUpdate;
        }
        public void RemoveLateUpdateMethod(Action onUpdate)
        {
            this.onLateUpdate -= onUpdate;
        }

        public void AddFixedUpdateMethod(Action onUpdate)
        {
            this.onFixedUpdate += onUpdate;
        }
        public void RemoveFixedUpdateMethod(Action onUpdate)
        {
            this.onFixedUpdate -= onUpdate;
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
            TimeCounter.Clear();
            AfterCallBack.Clear();
        }

        private void UpdateAfterAction()
        {
            //由于有删除操作，使用由大向小遍历，保证下标存在
            for (int i = TimeCounter.Count - 1; i >= 0; i--)
            {
                TimeCounter[i] -= Time.deltaTime;
                if (TimeCounter[i] <= 0)
                {
                    AfterCallBack[i]?.Invoke();
                    AfterCallBack.RemoveAt(i);
                    TimeCounter.RemoveAt(i);
                }
            }
        }
    }

}