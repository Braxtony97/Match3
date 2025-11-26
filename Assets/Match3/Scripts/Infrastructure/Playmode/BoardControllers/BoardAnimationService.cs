using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoardAnimationService
{
    private struct FallInfo
    {
        public TileView tile;
        public Vector2 oldPos;
        public Vector2 newPos;
    }
    
    private List<FallInfo> _fallInfos = new List<FallInfo>();
    
    private readonly IBoard _board;
    private readonly IBoardView _boardView;
    private readonly BoardMatchFinder _finder;

    public BoardAnimationService(IBoard board, IBoardView boardView, BoardMatchFinder finder)
    {
        _board = board;
        _boardView = boardView;
        _finder = finder;
    }
    
    public void CacheFallAnimations()
    {
        TileView[,] tiles = _boardView.GetTiles();

        for (int col = 0; col < _board.Width; col++)
        {
            for (int row = _board.Height - 1; row >= 0; row--)
            {
                if (_board.Get(row, col) == -1)
                    continue;
                
                int newRow = row;
                while (newRow + 1 < _board.Height && _board.Get(newRow + 1, col) == -1)
                {
                    newRow++;
                }

                if (newRow != row)
                {
                    TileView tile = tiles[row, col];
                    if (tile == null) continue;

                    _fallInfos.Add(new FallInfo
                    {
                        tile = tile,
                        oldPos = tile.RectTransform.anchoredPosition,
                        newPos = _boardView.GetTilePositionInUI(newRow, col)
                    });
                }
            }
        }
    }
    
    public void AnimateGravity(Action onComplete)
    {
        Sequence seq = DOTween.Sequence();

        foreach (var info in _fallInfos)
        {
            info.tile.RectTransform.anchoredPosition = info.oldPos;

            seq.Join(
                info.tile.RectTransform
                    .DOAnchorPos(info.newPos, 0.35f)
                    .SetEase(Ease.OutQuad)
            );
        }

        seq.OnComplete(() => { onComplete?.Invoke(); });
    }

    public void ClearFallInfo() => 
        _fallInfos.Clear();
}