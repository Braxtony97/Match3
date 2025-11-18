using UnityEngine;

public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
{
    private Game _game;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        LoadGame();
    }

    private void LoadGame()
    {
        _game = new Game(this);
        _game.StateMachine.Enter<BootstrapState>();
    }
}
