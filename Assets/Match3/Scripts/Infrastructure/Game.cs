public class Game
{
    public readonly GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner)
    {
        StateMachine = new GameStateMachine();
    }
}