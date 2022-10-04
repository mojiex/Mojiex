#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/10/4
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

namespace Mojiex
{
    public class FSM
    {
        private FSMBaseState m_currentState;
        private FSMBaseState m_defaultState;
        private FSMBaseState m_nextState;

        public void Execute()
        {
            if (m_currentState != null)
            {
                m_currentState.Execute();
            }
            else if (m_nextState != null)
            {
                ChangeState(m_nextState);
            }
            else if (m_defaultState != null)
            {
                ChangeState(m_defaultState);
            }
        }

        public void ChangeState(FSMBaseState state)
        {
            if (state == null || (m_currentState != null && m_currentState.priority > state.priority))
            {
                return;
            }
            m_currentState?.Exit();
            state.Enter();
            m_currentState = state;
        }

        public void ChangeToNextState()
        {
            if (m_nextState != null)
            {
                ChangeState(m_nextState);
            }
        }

        public void ChangeToDefaultState()
        {
            if (m_defaultState != null)
            {
                ChangeState(m_defaultState);
            }
        }
        public void StopCurrentState()
        {
            m_currentState?.Exit();
            m_currentState = null;
        }
        public FSMBaseState GetCurrentState() => m_currentState;
        public void SetDefaultState(FSMBaseState state) => m_defaultState = state;
        public void SetNextState(FSMBaseState state) => m_nextState = state;
        public int GetCurrentStatePriority() => m_currentState.priority;
    }
}