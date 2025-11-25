using UnityEngine;

public class BoardView : MonoBehaviour
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

    public void CreateGrid(IBoard board)
    {
        _board = board; 
        _tilePool.InitPool(_tilePrefab, _gridParent);
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

    private void GenerateGridView(IBoard board)
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

    private void SetTilePosition(IBoard board, TileView tile, int row, int col)
    {
        float totalWidth = _board.Width * (_tileSize + _spacing) - _spacing;
        float totalHeight = _board.Height * (_tileSize + _spacing) - _spacing;

        float startX = -totalWidth / 2f;
        float startY = totalHeight / 2f;
        
        float x = startX + col * (_tileSize + _spacing);
        float y = startY - row * (_tileSize + _spacing);
        
        RectTransform rect = tile.RectTransform;
        rect.anchoredPosition = new Vector2(x, y);
    }

    public void SwapTiles(int row, int col, int targetRow, int targetCol)
    {
        TileView temp = _gridViews[row, col];
        
        _gridViews[row, col] = _gridViews[targetRow, targetCol];
        _gridViews[targetRow, targetCol] = temp;
        
        _gridViews[row, col].SetPositionInUI(row, col);
        _gridViews[targetRow, targetCol].SetPositionInUI(targetRow, targetCol);
    }

    public void ClearTile(int row, int col)
    {
        TileView tile = _gridViews[row, col];
        if (tile == null)
            return; 

        _tilePool.ReturnTileToPool(tile);
        _gridViews[row, col] = null;
    }
}