using DG.Tweening;
using UnityEngine;

public class BoardController
{
    private IBoard _board;
    private IBoardView _view;
    
    private readonly BoardMatchFinder _finder;
    private readonly BoardGravityService _graviryService;
    private readonly BoardViewUpdater _viewUpdater;
    private readonly BoardAnimationService _animationService;

    public BoardController(IBoard board, IBoardView view, BoardMatchFinder finder, BoardGravityService gravityService,
        BoardViewUpdater viewUpdater, BoardAnimationService animationService)
    {
        _board = board;
        _view = view;
        _finder = finder;
        _graviryService = gravityService;
        _viewUpdater = viewUpdater;
        _animationService =  animationService;
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
        
        if (!_finder.IsInsideGrid(targetRow, targetCol))
            return;

        TrySwap(row, col, targetRow, targetCol);
    }

    private void TrySwap(int row, int col, int targetRow, int targetCol)
    {
        TileView[,] grid = _view.GetTiles();
        
        TileView tileA = grid[row, col];
        TileView tileB = grid[targetRow, targetCol];
        
        SwapInModel(row, col, targetRow, targetCol);
        
        bool hasMatch = HasMatchWithSwappedTiles(row, col, targetRow, targetCol);

        if (hasMatch)
        {
            Tween tween = _view.SwapTiles(row, col, targetRow, targetCol);
            tween.OnComplete(() => RemoveMatchesAndApplyGravity());
        }
        else
        {
            SwapInModel(row, col, targetRow, targetCol);
            
            TileView a = _view.GetTiles()[row, col];
            TileView b = _view.GetTiles()[targetRow, targetCol];

            Tween back = _view.AnimateSwapBack(tileA, tileB, 0.2f);
            back.OnComplete(() => {});
        }
    }

    private void RemoveMatchesAndApplyGravity()
    {
        MatchResult match = _finder.FindAllMatches();
    
        if (match.HasMatches)
        {
            RemoveMatches(match);
            ApplyGravityAndFill();
        }
        else
        {
            Debug.Log("No more matches after gravity");
        }
    }

    private bool HasMatchWithSwappedTiles(int row1, int col1, int row2, int col2)
    {
        return _finder.CheckMatchesForTile(row1, col1) || _finder.CheckMatchesForTile(row2, col2);
    }

    private void SwapInModel(int row, int col, int targetRow, int targetCol)
    {
        int tempTile = _board.Get(row, col);
        int targetTile = _board.Get(targetRow, targetCol);
        
        _board.Set(row, col, targetTile);
        _board.Set(targetRow, targetCol, tempTile);
    }
    
    private void RemoveMatches(MatchResult result)
    {
        foreach (var pos in result.MatchedTiles)
        {
            _board.Set(pos.x, pos.y, -1); 
            _view.ClearTile(pos.x, pos.y); 
        }
    }
    
    private void ApplyGravityAndFill()
    {
        _animationService.ClearFallInfo();

        _graviryService.CollapseBoard();
        _graviryService.FillEmptyTiles();
        
        _viewUpdater.UpdateView(HandleSwipe);

        _animationService.CacheFallAnimations();
        _animationService.AnimateGravity(() =>
        {
            MatchResult match = _finder.FindAllMatches();
            if (match.HasMatches)
            {
                RemoveMatches(match);
                ApplyGravityAndFill();
            }
        });
    }
}