using System;

namespace Code.States
{
    public interface IStateMachine
    {
        IState currentState { get; }
        IState pausedState { get; }
        IState nextState { get;  }
        void Play<T>(TransitionDelegate<T> transition = null) where T : IState;
        void Play(Type state);
        void Play(IState state, TransitionDelegate<IState> transition = null);
        T GetState<T>() where T : IState;
        void Update();
        void Stop();
        void Resume();
        void Pause();
        bool Restore();
        void Save(IState state);
    }
}