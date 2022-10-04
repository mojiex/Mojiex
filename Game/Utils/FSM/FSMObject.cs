#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/10/4
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mojiex
{
    public class FSMObject : MonoBehaviour
    {
        private FSM fsm = new FSM();

        protected virtual void Awake()
        {
            SupportBehavior.Inst.AddUpdateMethod(FSMUpdate);
        }

        protected virtual void FSMUpdate()
        {
            fsm.Execute();
        }

        protected virtual void OnDestroy()
        {
            if (!SupportBehavior.isDestory)
                SupportBehavior.Inst.RemoveUpdateMethod(FSMUpdate);
        }

        public virtual void ChangeState(FSMBaseState state) => fsm.ChangeState(state);
        public virtual void SetDefaultState(FSMBaseState state) => fsm.SetDefaultState(state);
        public virtual void SetNextState(FSMBaseState state) => fsm.SetNextState(state);
        public FSMBaseState GetCurrentState() => fsm.GetCurrentState();
        public int GetCurrentStatePriority() => GetCurrentState().priority;
        public void StopCurrentState() => fsm.StopCurrentState();
    }
}