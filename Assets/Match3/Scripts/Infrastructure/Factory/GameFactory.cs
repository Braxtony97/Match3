using UnityEngine;

public class GameFactory : IGameFactory
{
    private readonly IAssetProvider _assetProvider;
    private readonly IGameStateMachine _gameStateMachine;

    public GameFactory(IAssetProvider assetProvider, IGameStateMachine gameStateMachine)
    {
        _assetProvider = assetProvider;
        _gameStateMachine = gameStateMachine;
    }

    public GameObject CreateGridView(string prefabPath)
    {
        var gameObject = _assetProvider.Instantiate(prefabPath);
        return gameObject;
    }
}