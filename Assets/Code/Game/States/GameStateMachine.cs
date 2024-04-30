using System;
using Code.Services;
using Code.States;
using UnityEngine;

namespace Code.Game.States
{
    public class GameStateMachine : MonoBehaviour, IStateMachine, IService
    {
        private StateMachine m_StateMachine;
        public IState currentState => m_StateMachine.currentState;
        public IState pausedState => m_StateMachine.pausedState;
        public IState nextState => m_StateMachine.nextState;
        
        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        private void Start()
        {
            m_StateMachine = new StateMachine(name, GetComponentsInChildren<IState>());
            m_StateMachine.Play<BootstrapState>();
        }

        public void Play<T>(TransitionDelegate<T> transition = null) where T : IState
        {
            m_StateMachine.Play<T>(transition);
        }

        public void Play(Type type)
        {
            m_StateMachine.Play(type);
        }

        public void Play(IState state, TransitionDelegate<IState> transition = null)
        {
            m_StateMachine.Play(state, transition);
        }

        public T GetState<T>() where T : IState
        {
            return m_StateMachine.GetState<T>();
        }

        public void Update()
        {
            m_StateMachine.Update();
        }

        public void Stop()
        {
            m_StateMachine.Stop();
        }

        public void Resume()
        {
            m_StateMachine.Resume();
        }

        public void Pause()
        {
            m_StateMachine.Pause();
        }

        public void Save(IState state)
        {
            m_StateMachine.Save(state);
        }

        public bool Restore()
        {
           return m_StateMachine.Restore();
        }
    }
}