using System;
using System.Collections.Generic;
using System.Reflection;
using Code.IO;
using UnityEngine;

namespace Code.States
{
    [System.Serializable]
    public class StateMachine : IStateMachine
    {
        private List<IState> m_States;
        public IState currentState { get; private set; }
        public IState pausedState { get; private set; }
        public IState nextState { get; private set; }

        [System.Serializable]
        public class StateData
        {
            public bool m_Save;
            public string m_Type;

            public StateData()
            {
                m_Save = false;
                m_Type = "";
            }

            public StateData(IState state)
            {
                m_Save = true;
                m_Type = state.GetType().FullName;
            }
        }

        private PlayerPrefsProperty<StateData> m_SavedState;

        public StateMachine(string name)
        {
            m_States = new List<IState>();
            m_SavedState = new PlayerPrefsProperty<StateData>($"{name}_{nameof(m_SavedState)}");
        }

        public StateMachine(string name, IEnumerable<IState> states)
        {
            m_States = new List<IState>(states);
            m_SavedState = new PlayerPrefsProperty<StateData>($"{name}_{nameof(m_SavedState)}");
        }

        public void Play<T>(TransitionDelegate<T> transitionDelegate = null) where T : IState
        {
            var state = m_States.Find(x => x is T);
            if (state != null)
            {
                if (currentState != state)
                {
                    nextState = state;
                    if (currentState != null && currentState is IStopState stopState)
                    {
                        stopState.OnStop(this);
                        Debug.Log($"On Stop: {stopState}");
                    }

                    transitionDelegate?.Invoke((T)state);
                    currentState = state;
                    if (state is IPlayState playState)
                    {
                        Debug.Log($"OnPlay: {playState}");
                        playState.OnPlay(this);
                    }
                }
            }
        }

        public void Play(Type type)
        {
            IState state = m_States.Find(x => x.GetType().IsAssignableFrom(type));
            Play(state);
        }

        public void Play(IState state, TransitionDelegate<IState> transitionDelegate = null)
        {
            if (state != null)
            {
                if (currentState != state)
                {
                    nextState = state;
                    if (currentState != null && currentState is IStopState stopState)
                    {
                        stopState.OnStop(this);
                        Debug.Log($"On Stop: {stopState}");
                    }

                    transitionDelegate?.Invoke(state);
                    currentState = state;
                    if (state is IPlayState playState)
                    {
                        Debug.Log($"OnPlay: {playState}");
                        playState.OnPlay(this);
                    }
                }
            }
        }

        public T GetState<T>() where T : IState
        {
            return (T)m_States.Find(x => x is T);
        }

        public void Update()
        {
            if (currentState != null && currentState is IUpdateState updateState)
            {
                updateState.OnUpdate(this);
            }
        }

        public void Stop()
        {
            if (currentState != null && currentState is IStopState stopState)
            {
                stopState.OnStop(this);
                Debug.Log($"OnStop: {stopState}");
            }
        }

        public void Resume()
        {
            if (pausedState != null && pausedState == currentState && pausedState is IPauseState localPausedState)
            {
                Debug.Log($"OnResume: {localPausedState}");
                localPausedState.OnResume(this);
                pausedState = null;
            }
        }

        public void Pause()
        {
            if (currentState != null && currentState is IPauseState pauseState)
            {
                pausedState = currentState;
                Debug.Log($"OnPlay: {pauseState}");
                pauseState.OnPause(this);
            }
        }

        public void Save(IState state)
        {
            m_SavedState.value = new StateData(state);
        }

        public bool Restore()
        {
            if (m_SavedState.value.m_Save)
            {
                var findState = m_States.Find(x => x.GetType().FullName == m_SavedState.value.m_Type);
                Play(findState);
                return true;
            }
            return false;
        }
    }
}