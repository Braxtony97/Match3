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
        
        MatchResult match = FindAllMatches();

        if (match.HasMatches)
        {
            RemoveMatches(match);
            ApplyGravityAndFill();
        }
        else
        {
            // Откат
            SwapInModel(row, col, targetRow, targetCol);
            Debug.Log("No match. Swap reverted.");
        }
    }

    private MatchResult FindAllMatches()
    {
        MatchResult result = new MatchResult();
    
        FindHorizontalMatches(result);
        FindVerticalMatches(result);

        return result;
    }

    private void SwapInModel(int row, int col, int targetRow, int targetCol)
    {
        int temp = _board.Get(row, col);
        
        _board.Set(row, col, _board.Get(targetRow, targetCol));
        _board.Set(targetRow, targetCol, temp);
        
        _view.SwapTiles(row, col, targetRow, targetCol);
    }
    
    private void FindHorizontalMatches(MatchResult result)
    {
        for (int row = 0; row < _board.Height; row++)
        {
            int count = 1;

            for (int col = 1; col < _board.Width; col++)
            {
                if (_board.Get(row, col) == _board.Get(row, col - 1))
                    count++;
                else
                {
                    if (count >= 3)
                    {
                        for (int k = 0; k < count; k++)
                            result.MatchedTiles.Add(new Vector2Int(row, col - 1 - k));
                    }
                    count = 1;
                }
            }
            
            if (count >= 3)
            {
                for (int k = 0; k < count; k++)
                    result.MatchedTiles.Add(new Vector2Int(row, _board.Width - 1 - k));
            }
        }
    }
    
    private void FindVerticalMatches(MatchResult result)
    {
        for (int col = 0; col < _board.Width; col++)
        {
            int count = 1;

            for (int row = 1; row < _board.Height; row++)
            {
                if (_board.Get(row, col) == _board.Get(row - 1, col))
                    count++;
                else
                {
                    if (count >= 3)
                    {
                        for (int k = 0; k < count; k++)
                            result.MatchedTiles.Add(new Vector2Int(row - 1 - k, col));
                    }
                    count = 1;
                }
            }
            
            if (count >= 3)
            {
                for (int k = 0; k < count; k++)
                    result.MatchedTiles.Add(new Vector2Int(_board.Height - 1 - k, col));
            }
        }
    }
    
    private void RemoveMatches(MatchResult result)
    {
        foreach (var pos in result.MatchedTiles)
        {
            _board.Set(pos.x, pos.y, -1); // -1 = пустая ячейка
            _view.ClearTile(pos.x, pos.y); // сделаешь анимацию позже
        }
    }
    
    private void ApplyGravityAndFill()
    {
        CollapseBoard();
        FillEmptyTiles();
        UpdateView();
    }
    
    private void CollapseBoard()
    {
        for (int col = 0; col < _board.Width; col++)
        {
            int writeRow = _board.Height - 1;

            for (int row = _board.Height - 1; row >= 0; row--)
            {
                int val = _board.Get(row, col);

                if (val != -1)
                {
                    _board.Set(writeRow, col, val);
                    writeRow--;
                }
            }

            // заполняем пустые сверху
            for (int row = writeRow; row >= 0; row--)
                _board.Set(row, col, -1);
        }
    }
    
    private void FillEmptyTiles()
    {
        int types = _view.TileTypesCount;

        for (int row = 0; row < _board.Height; row++)
        {
            for (int col = 0; col < _board.Width; col++)
            {
                if (_board.Get(row, col) == -1)
                {
                    int newTile = UnityEngine.Random.Range(0, types);
                    _board.Set(row, col, newTile);
                }
            }
        }
    }
    
    private void UpdateView()
    {
        var tiles = _view.GetTiles();

        for (int row = 0; row < _board.Height; row++)
        {
            for (int col = 0; col < _board.Width; col++)
            {
                int tileId = _board.Get(row, col);

                // пустая ячейка
                if (tileId == -1)
                {
                    if (tiles[row, col] != null)
                    {
                        _view.ClearTile(row, col);
                    }
                    continue;
                }

                // нет TileView — создаём
                if (tiles[row, col] == null)
                {
                    TileView tv = _view.SpawnTile(row, col, tileId);
                    tiles[row, col] = tv;
                }
                else
                {
                    tiles[row, col].SetSprite(_view.GetSprite(tileId));
                    tiles[row, col].SetPositionInUI(row, col);
                }
            }
        }
    }
}