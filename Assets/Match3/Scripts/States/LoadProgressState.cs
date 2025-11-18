
public class LoadProgressState : IState 
//State, отвечающий за загрузку данных пользователя
{
    private readonly GameStateMachine _gameStateMachine;

    private string _playmodeScene = GameEnums.SceneType.Gameplay.ToString(); // Temp MOCK

    public LoadProgressState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public void Enter()
    {
        LoadProgressOrInitNew();

        _gameStateMachine.Enter<LoadSceneState, string>(_playmodeScene);
    }

    private void LoadProgressOrInitNew()
    {
        //Загрузка сохраненных данных или (if null) - создание новых
    }

    public void Exit()
    {
    }
}
