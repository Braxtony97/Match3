using UnityEngine;

public class Board
{
    public int Width => _width;
    public int Height => _height; 
    
    private int _width;
    private int _height;
    private int[,] _grid;

    public Board(int width, int height)
    {
        _width = width;
        _height = height; 
        _grid = new int[height, width];
    }

    public int Get(int row, int col) => _grid[row, col];
    public void Set(int row, int col, int value) => _grid[row, col] = value;

    public void FillRandom(int tileTypesCount)
    {
        for (int row = 0; row < _height; row++)
        for (int col = 0; col < _width; col++)
            _grid[row, col] = Random.Range(0, tileTypesCount);
    }
    
    public void Swap(int r1, int c1, int r2, int c2)
    {
        int temp = _grid[r1, c1];
        _grid[r1, c1] = _grid[r2, c2];
        _grid[r2, c2] = temp;
    }
}