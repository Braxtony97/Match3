using UnityEngine;

public class BoardMatchFinder
{
    private readonly IBoard _board;

    public BoardMatchFinder(IBoard board)
    {
        _board = board;
    }
    
    public bool IsInsideGrid(int targetRow, int targetCol)
    {
        bool isInsideRow = targetRow >= 0 && targetRow < _board.Height;
        bool isInsideCol = targetCol >= 0 && targetCol < _board.Width;
        
        return isInsideRow && isInsideCol;
    }
    
    public bool CheckMatchesForTile(int row, int col)
    {
        int tileType = _board.Get(row, col);
        
        int horizontalCount = 1 + CountSameTiles(row, col, Vector2Int.left, tileType) 
                                + CountSameTiles(row, col, Vector2Int.right, tileType);
        
        int verticalCount = 1 + CountSameTiles(row, col, Vector2Int.down, tileType)
                              + CountSameTiles(row, col, Vector2Int.up, tileType);
    
        return horizontalCount >= 3 || verticalCount >= 3;
    }

    private int CountSameTiles(int startRow, int startCol, Vector2Int direction, int tileType)
    {
        int count = 0;
        int row = startRow + direction.y;
        int col = startCol + direction.x;
    
        while (IsInsideGrid(row, col) && _board.Get(row, col) == tileType)
        {
            count++;
            row += direction.y;
            col += direction.x;
        }
    
        return count;
    }

    public MatchResult FindAllMatches()
    {
        MatchResult result = new MatchResult();
    
        FindHorizontalMatches(result);
        FindVerticalMatches(result);

        return result;
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
}