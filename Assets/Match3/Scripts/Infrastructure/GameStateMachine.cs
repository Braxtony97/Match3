using System;
using System.Collections.Generic;

public class GameStateMachine : IGameStateMachine
{
    private readonly Dictionary<Type, IExitableState> _states;
    private IExitableState _currentState;
    
    private readonly BoardConfig _boardConfig;

    public GameStateMachine(SceneLoader sceneLoader, ServiceLocator serviceLocator)
    {
        _states = new Dictionary<Type, IExitableState>()
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, serviceLocator),
            [typeof(LoadProgressState)] = new LoadProgressState(this),
            [typeof(LoadSceneState)] = new LoadSceneState(this, sceneLoader, serviceLocator.Resolve<IGameFactory>()),
            [typeof(PlayModeState)] = new PlayModeState(this, sceneLoader)
        };
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : IPayloadState<TPayload>
    {
        var state = GetState<TState>();
        state.Enter(payload);
    }

    public void Enter<TState>() where TState : IState
    {
        TState state = GetState<TState>();
        state.Enter();
    }

    private TState GetState<TState>() where TState : IExitableState
    {
        _currentState?.Exit();
        TState state = (TState)_states[typeof(TState)];
        _currentState = state;
        return state;
    }
}