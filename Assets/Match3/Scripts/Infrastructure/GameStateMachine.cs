using System;
using System.Collections.Generic;

public class GameStateMachine : IGameStateMachine
{
    private readonly Dictionary<Type, IState> _states;
    private IState _currentState;

    public GameStateMachine(SceneLoader sceneLoader, ServiceLocator serviceLocator)
    {
        _states = new Dictionary<Type, IState>()
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, serviceLocator)
        };
    }

    public void Enter<TState>()
    {
        IState state = GetState(typeof(TState));
        state.Enter();
    }

    private IState GetState(Type stateType)
    {
        _currentState?.Exit();
        IState state = _states[stateType];
        _currentState = state;
        return state;
    }
}