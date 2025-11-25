using UnityEngine;

public class BoardView : MonoBehaviour
{
    public TileView[,] GetTiles() => _gridViews;
    public float TileSize => _tileSize;
    public float Spacing => _spacing;
    
    [SerializeField] private TileStaticData[] _tileTypes;
    [SerializeField] private TileView _tilePrefab;
    [SerializeField] private Transform _gridParent;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private float _tileSize;
    [SerializeField] private float _spacing;

    private TileView[,] _gridViews;

    public void CreateGrid(Board board)
    {
        _gridViews = new TileView[board.Height, board.Width];
        GenerateGridView(board);
    }

    private void GenerateGridView(Board board)
    {
        float totalWidth = board.Width * (_tileSize + _spacing) - _spacing;
        float totalHeight = board.Height * (_tileSize + _spacing) - _spacing;

        float startX = -totalWidth / 2f;
        float startY = totalHeight / 2f;

        for (int row = 0; row < board.Height; row++)
        {
            for (int col = 0; col < board.Width; col++)
            {
                TileView tile = Instantiate(_tilePrefab, _gridParent);
                TileView tileView = tile.GetComponent<TileView>();
                tileView.Construct(this, _mainCanvas);
                RectTransform rect = tileView.RectTransform;

                float x = startX + col * (_tileSize + _spacing);
                float y = startY - row * (_tileSize + _spacing);

                rect.anchoredPosition = new Vector2(x, y);

                tile.SetPositionInUI(row, col);
                
                int tileId = board.Get(row, col);
                TileStaticData data = _tileTypes[tileId];
                tile.SetSprite(data.Sprite);

                _gridViews[row, col] = tile;
            }
        }
    }
    
    public void SwapTiles(int r1, int c1, int r2, int c2)
    {
        TileView t1 = _gridViews[r1, c1];
        var t2 = _gridViews[r2, c2];

        Sprite s1 = t1.Sprit;
        Sprite s2 = t2.Sprit;

        t1.SetSprite(s2);
        t2.SetSprite(s1);
    }
}