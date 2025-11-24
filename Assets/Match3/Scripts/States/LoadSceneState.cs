using System;
using UnityEngine;

public class LoadSceneState : IPayloadState<string>
{
    private readonly SceneLoader _sceneLoader;
    private readonly IGameFactory _gameFactory;
    private readonly GameStateMachine _stateMachine;
    private readonly IServiceProvider _serviceProvider;

    public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory gameFactory)
    {
        _sceneLoader = sceneLoader;
        _gameFactory = gameFactory;
        _stateMachine = stateMachine;
    }

    public void Enter(string payload)
    {
        _sceneLoader.Load(payload, onLoaded: OnLoaded);
    }

    private void OnLoaded()
    {
        InitUIRoot();
        InitGameWorld();
        
        _stateMachine.Enter<PlayModeState>();
    }

    private void InitUIRoot()
    {
    }

    private void InitGameWorld()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        BoardConfig config = ServiceLocator.Instance.Resolve<BoardConfig>();
        Board board = new Board(config.Width, config.Height);
        board.FillRandom(config.UniqueTiles);
        GameObject manager = _gameFactory.CreateGridManager(ResourcesPaths.GridViewPath);
        BoardView boardView = manager.GetComponent<BoardView>();
        boardView.CreateGrid(board);
    }

    public void Exit()
    {
    }
}