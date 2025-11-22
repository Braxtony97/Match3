using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    private int[,] _grid;

    public void CreateGrid()
    {
        _grid = new int[_width, _height];
        Debug.Log("Creating Grid");
    }    
}