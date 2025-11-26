using UnityEngine;

public interface IGameFactory : IService
{
    GameObject CreateGridView(string prefabPath);
}