public class BootstrapState : IState
// State, отвечающий за регистрацию сервисов в ServiceLocator
{
    private ServiceLocator _serviceLocator;
    private GameStateMachine _stateMachine;
    private SceneLoader _sceneLoader;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, ServiceLocator serviceLocator)
    {
        _serviceLocator = serviceLocator;
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;

        RegisterService();
    }

    private void RegisterService()
    {
        _serviceLocator.Register<IGameStateMachine>(_stateMachine);
        _serviceLocator.Register<SceneLoader>(_sceneLoader);
    }

    public void Enter()
    {
        _stateMachine.Enter<LoadProgressState>();
    }

    public void Exit()
    {
    }
}
