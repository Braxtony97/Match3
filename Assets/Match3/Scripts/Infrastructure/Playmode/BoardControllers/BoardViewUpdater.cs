using System;
using UnityEngine;

public class BoardViewUpdater
{
    private readonly IBoard _board;
    private readonly IBoardView _boardView;

    public BoardViewUpdater(IBoard board, IBoardView boardView)
    {
        _board = board;
        _boardView = boardView;
    }

    public void UpdateView(Action<int, int, Vector2Int> subscribeSwipe)
    {
        var tiles = _boardView.GetTiles();

        for (int row = 0; row < _board.Height; row++)
        {
            for (int col = 0; col < _board.Width; col++)
            {
                int tileId = _board.Get(row, col);
                
                if (tileId == -1)
                {
                    if (tiles[row, col] != null)
                    {
                        _boardView.ClearTile(row, col);
                    }
                    continue;
                }
                
                if (tiles[row, col] == null)
                {
                    TileView tile = _boardView.SpawnTile(row, col, tileId);
                    tile.OnSwipe -= subscribeSwipe;
                    tile.OnSwipe += subscribeSwipe;
                    tiles[row, col] = tile;
                }
                else
                {
                    tiles[row, col].SetSprite(_boardView.GetSprite(tileId));
                    tiles[row, col].SetPositionInUI(row, col);
                }
            }
        }
    }       
}