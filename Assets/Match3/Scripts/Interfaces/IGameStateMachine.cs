public interface IGameStateMachine : IService
{
    void Enter<TState, TPayload>(TPayload payload) where TState : IPayloadState<TPayload>;
    void Enter<TState>() where TState : IState;
}