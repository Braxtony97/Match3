using UnityEngine;

public class BoardController
{
    private Board _board;
    private BoardView _view;

    public BoardController(Board board, BoardView view)
    {
        _board = board;
        _view = view;
    }

    public void Initialize()
    {
        TileView[,] tiles = _view.GetTiles();

        for (int row = 0; row < _board.Height; row++)
        {
            for (int col = 0; col < _board.Width; col++)
            {
                tiles[row, col].OnSwipe += HandleSwipe;
            }
        }
    }

    private void HandleSwipe(int row, int col, Vector2Int direction)
    {
        int targetRow = row - direction.y;
        int targetCol = col + direction.x;
        
        if (!IsInsideGrid(targetRow, targetCol))
            return;

        TrySwap(row, col, targetRow, targetCol);
    }
    
    private bool IsInsideGrid(int targetRow, int targetCol)
    {
        bool isInsideRow = targetRow >= 0 && targetRow < _board.Height;
        bool isInsideCol = targetCol >= 0 && targetCol < _board.Width;
        
        return isInsideRow && isInsideCol;
    }

    private void TrySwap(int row, int col, int targetRow, int targetCol)
    {
        SwapInModel(row, col, targetRow, targetCol);

        if (HasMatches())
        {
            // TODO: запустить удаление и анимации
            Debug.Log("Match found!");
        }
        else
        {
            // Откат
            SwapInModel(row, col, targetRow, targetCol);
            Debug.Log("No match. Swap reverted.");
        }
    }
    
    private void SwapInModel(int row, int col, int targetRow, int targetCol)
    {
        int temp = _board.Get(row, col);
        _board.Set(row, col, _board.Get(targetRow, targetCol));
        _board.Set(targetRow, targetCol, temp);

        // TODO: обновление UI (позже анимации)
        _view.SwapTiles(row, col, targetRow, targetCol);
    }
    
    private bool HasMatches()
    {
        return HasHorizontalMatches() || HasVerticalMatches();
    }
    
    private bool HasHorizontalMatches()
    {
        for (int row = 0; row < _board.Height; row++)
        {
            int count = 1;

            for (int col = 1; col < _board.Width; col++)
            {
                if (_board.Get(row, col) == _board.Get(row, col - 1))
                    count++;
                else
                    count = 1;

                if (count >= 3)
                    return true;
            }
        }
        return false;
    }
    
    private bool HasVerticalMatches()
    {
        for (int col = 0; col < _board.Width; col++)
        {
            int count = 1;

            for (int row = 1; row < _board.Height; row++)
            {
                if (_board.Get(row, col) == _board.Get(row - 1, col))
                    count++;
                else
                    count = 1;

                if (count >= 3)
                    return true;
            }
        }
        return false;
    }
}