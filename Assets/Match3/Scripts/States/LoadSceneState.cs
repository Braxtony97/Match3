using System;
using UnityEngine;

public class LoadSceneState : IPayloadState<string>
{
    private readonly SceneLoader _sceneLoader;
    private readonly IGameFactory _gameFactory;
    private readonly GameStateMachine _stateMachine;
    private readonly IServiceProvider _serviceProvider;
    private readonly ServiceLocator _serviceLocator;
    private readonly BoardConfig _boardConfig;
    private IBoard _board;

    public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory)
    {
        _sceneLoader = sceneLoader;
        _gameFactory = gameFactory;
        _stateMachine = stateMachine;
        
        _serviceLocator = ServiceLocator.Instance;
        _boardConfig = _serviceLocator.Resolve<BoardConfig>();
    }

    public void Enter(string payload)
    {
        _sceneLoader.Load(payload, onLoaded: OnLoaded);
    }

    private void OnLoaded()
    {
        InitGameWorld();
        
        _stateMachine.Enter<PlayModeState>();
    }

    private void InitGameWorld()
    {
        PrepareBoard();
        PrepareBoardView();
    }
    
    private void PrepareBoard()
    {
        _board = new BoardModel(_boardConfig.Width, _boardConfig.Height);
        _board.FillRandom(_boardConfig.UniqueTiles);
        _serviceLocator.Register<IBoard>(_board);
    }

    private void PrepareBoardView()
    {
        GameObject projectContext = _gameFactory.CreateGridView(ResourcesPaths.ProjectContextPath);
        BoardView boardView = projectContext.GetComponent<BoardView>();
        boardView.CreateGrid(_board, _boardConfig);
        _serviceLocator.Register<IBoardView>(boardView);
    }

    public void Exit()
    {
    }
}