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
        ControllersInit();
    }
    
    private void ResolveServices()
    {
        _serviceLocator = ServiceLocator.Instance;
        _board =  _serviceLocator.Resolve<IBoard>();
        _boardView =  _serviceLocator.Resolve<IBoardView>();
    }

    private void ControllersInit() 
    {
        BoardMatchFinder finder = new BoardMatchFinder(_board);
        BoardGravityService gravityService = new BoardGravityService(_board, _boardView);
        BoardViewUpdater viewUpdater = new BoardViewUpdater(_board, _boardView);
        BoardAnimationService animationService = new BoardAnimationService(_board, _boardView, finder);
        
        BoardController boardController = new  BoardController(_board, _boardView, finder, gravityService, viewUpdater, animationService);
        boardController.Initialize();
    }

    public void Exit()
    {
    }
}