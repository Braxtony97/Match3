using UnityEngine;

public class LoadSceneState : IPayloadState<string>
//State, отвечающий за создание объектов на сцене с помощью GameFactory
{
    private readonly SceneLoader _sceneLoader;
    private readonly IGameFactory _gameFactory;
    private readonly GameStateMachine _stateMachine;

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
        Debug.Log("Создание UI");
    }

    private void InitGameWorld()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        GameObject manager = _gameFactory.CreateGridManager(ResourcesPaths.GridManagerPath);
        GridManager gridManager = manager.GetComponent<GridManager>();
        gridManager.CreateGrid();
        
    }

    public void Exit()
    {
    }
}