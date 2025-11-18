using UnityEngine;

public class LoadSceneState : IPayloadState<string>
//State, отвечающий за создание объектов на сцене с помощью GameFactory
{
    private readonly SceneLoader _sceneLoader;
    private readonly GameStateMachine _stateMachine;

    public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
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
    }

    private void InitUIRoot()
    {
        Debug.Log("Создание UI");
    }

    private void InitGameWorld()
    {
        Debug.Log("Create grid");
    }

    public void Exit()
    {
    }
}
