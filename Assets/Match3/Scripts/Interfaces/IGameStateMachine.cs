public interface IGameStateMachine : IService
{
    void Enter<TState>();
}