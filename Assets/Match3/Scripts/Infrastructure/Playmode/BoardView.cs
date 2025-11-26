using DG.Tweening;
using UnityEngine;

public class BoardView : MonoBehaviour, IBoardView
{
    public TileView[,] GetTiles() => _gridViews;
    public float TileSize => _tileSize;
    public float Spacing => _spacing;
    
    public Sprite GetSprite(int tileId) => _tileTypes[tileId].Sprite;

    public int TileTypesCount => _tileTypes.Length;
    
    [SerializeField] private TileStaticData[] _tileTypes;
    [SerializeField] private TileView _tilePrefab;
    [SerializeField] private Transform _gridParent;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private float _tileSize;
    [SerializeField] private float _spacing;
    [SerializeField] private TilePool _tilePool;

    private TileView[,] _gridViews;
    private IBoard _board;

    public void CreateGrid(IBoard board, BoardConfig boardConfig)
    {
        _board = board; 
        _tilePool.InitPool(_tilePrefab, _gridParent, boardConfig);
        _gridViews = new TileView[board.Height, board.Width];
        GenerateGridView(board);
    }
    
    public TileView SpawnTile(int row, int col, int tileId)
    {
        TileView tile = _tilePool.GetTileFromPool();
        tile.Construct(this, _mainCanvas);

        SetTilePosition(null, tile, row, col);
        tile.SetPositionInUI(row, col);
 
        tile.SetSprite(_tileTypes[tileId].Sprite);
        tile.gameObject.SetActive(true);

        _gridViews[row, col] = tile;

        return tile;
    }

    public void GenerateGridView(IBoard board)
    {
        for (int row = 0; row < board.Height; row++)
        {
            for (int col = 0; col < board.Width; col++)
            {
                TileView tile = _tilePool.GetTileFromPool();
                tile.Construct(this, _mainCanvas);

                SetTilePosition(board, tile, row, col);
                tile.SetPositionInUI(row, col);
                
                int tileId = board.Get(row, col);
                TileStaticData data = _tileTypes[tileId];
                tile.SetSprite(data.Sprite);
                
                tile.gameObject.SetActive(true); 
                
                _gridViews[row, col] = tile;
            }
        }
    }

    public void SetTilePosition(IBoard board, TileView tile, int row, int col)
    {
        float totalWidth = _board.Width * (_tileSize + _spacing) - _spacing;
        float totalHeight = _board.Height * (_tileSize + _spacing) - _spacing;

        float startX = -totalWidth / 2f;
        float startY = totalHeight / 2f;
        
        float x = startX + col * (_tileSize + _spacing);
        float y = startY - row * (_tileSize + _spacing);
        
        tile.RectTransform.anchoredPosition = new Vector2(x, y);
    }

    public Tween SwapTiles(int row, int col, int targetRow, int targetCol)
    {
        TileView a = _gridViews[row, col];
        TileView b = _gridViews[targetRow, targetCol];

        Vector2 posA = GetTilePositionInUI(row, col);
        Vector2 posB = GetTilePositionInUI(targetRow, targetCol);

        Sequence seq = DOTween.Sequence();
        seq.Join(a.RectTransform.DOAnchorPos(posB, 0.2f));
        seq.Join(b.RectTransform.DOAnchorPos(posA, 0.2f));

        seq.OnComplete(() =>
        {
            _gridViews[row, col] = b;
            _gridViews[targetRow, targetCol] = a;

            a.SetPositionInUI(targetRow, targetCol);
            b.SetPositionInUI(row, col);
        });

        return seq;
    }
    
    public Tween AnimateSwap(TileView a, TileView b, float duration = 0.2f)
    {
        Vector2 aPos = a.RectTransform.anchoredPosition;
        Vector2 bPos = b.RectTransform.anchoredPosition;
        
        Sequence seq = DOTween.Sequence();
        seq.Join(a.RectTransform.DOAnchorPos(bPos, duration).SetEase(Ease.OutBack));
        seq.Join(b.RectTransform.DOAnchorPos(aPos, duration).SetEase(Ease.OutBack));

        return seq;
    }
    
    public Tween AnimateSwapBack(TileView a, TileView b, float duration = 0.2f)
    {
        Vector2 posA = GetTilePositionInUI(a.Row, a.Col);
        Vector2 posB = GetTilePositionInUI(b.Row, b.Col);

        Sequence seq = DOTween.Sequence();
        seq.Join(a.RectTransform.DOAnchorPos(posA, duration).SetEase(Ease.OutBack));
        seq.Join(b.RectTransform.DOAnchorPos(posB, duration).SetEase(Ease.OutBack));

        return seq; 
    }

    public void ClearTile(int row, int col)
    {
        TileView tile = _gridViews[row, col];
        if (tile == null)
            return; 

        _tilePool.ReturnTileToPool(tile);
        _gridViews[row, col] = null;
    }

    public Vector2 GetTilePositionInUI(int row, int col)
    {
        float totalWidth = _board.Width * (_tileSize + _spacing) - _spacing;
        float totalHeight = _board.Height * (_tileSize + _spacing) - _spacing;

        float startX = -totalWidth / 2f;
        float startY = totalHeight / 2f;

        float x = startX + col * (_tileSize + _spacing);
        float y = startY - row * (_tileSize + _spacing);

        return new Vector2(x, y);
    }
}