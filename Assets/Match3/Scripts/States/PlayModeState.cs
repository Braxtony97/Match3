public class PlayModeState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private ServiceLocator _serviceLocator;
    private IBoard _board;
    private IBoardView _boardView;

    public PlayModeState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
    }

    public void Enter()
    {
        ResolveServices();
        PlaymodeControllerInit();
    }

    private void PlaymodeControllerInit()
    {
        BoardController boardController = new  BoardController(_board, _boardView);
        boardController.Initialize();
    }

    private void ResolveServices()
    {
        _serviceLocator = ServiceLocator.Instance;
        
        _board =  _serviceLocator.Resolve<IBoard>();
        _boardView =  _serviceLocator.Resolve<IBoardView>();
    }

    public void Exit()
    {
    }
}