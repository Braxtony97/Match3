using UnityEngine;

public interface IGameFactory : IService
{
    GameObject CreateGridManager(string prefabPath);
}