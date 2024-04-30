namespace Code.States
{
    public delegate void TransitionDelegate<T>(T state) where T : IState;
}