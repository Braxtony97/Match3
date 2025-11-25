public class PlayModeState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;

    public PlayModeState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
    }

    public void Enter()
    {
        //BoardController boardController = new  BoardController(board, boardView);
        //boardController.Initialize();
    }

    public void Exit()
    {
    }
}