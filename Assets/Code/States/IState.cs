namespace Code.States
{
    public interface IState
    {
        
    }
    
    public interface IRestorable : IState
    {
        void Save();
        void Restore();
    }
    
    public interface IPlayState : IState
    {
        void OnPlay(IStateMachine stateMachine);
    }
    
    public interface IUpdateState : IState
    {
        void OnUpdate(IStateMachine stateMachine);
    }
    
    public interface IStopState : IState
    {
        void OnStop(IStateMachine stateMachine);
    }

    public interface IPauseState : IState
    {
        void OnResume(IStateMachine stateMachine);
        void OnPause(IStateMachine stateMachine);
    }
}