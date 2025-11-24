using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private TileStaticData[] _tileTypes;
    
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private TileView _tilePrefab;
    [SerializeField] private Transform _gridParent;
    
    [SerializeField] private float _tileSize;
    [SerializeField] private float _spacing;

    private int[,] _grid;
    private TileView[,] _gridViews;

    public void CreateGrid()
    {
        _grid = new int[_height, _width];
        _gridViews = new TileView[_height, _width]; 

        GenerateGridData();
        GenerateGridView();
    }

    private void GenerateGridData()
    {
        
    }

    private void GenerateGridView()
    {
        float totalWidth = _width * (_tileSize + _spacing) - _spacing;
        float totalHeight = _height * (_tileSize + _spacing) - _spacing;

        float startX = -totalWidth / 2f;
        float startY = totalHeight / 2f;

        for (int row = 0; row < _height; row++)
        {
            for (int col = 0; col < _width; col++)
            {
                TileView tile = Instantiate(_tilePrefab, _gridParent);
                RectTransform rect = tile.GetComponent<RectTransform>();

                float x = startX + col * (_tileSize + _spacing);
                float y = startY - row * (_tileSize + _spacing);

                rect.anchoredPosition = new Vector2(x, y);

                tile.SetPositionInUI(row, col);
                tile.SetSprite(_grid[row, col]);

                _gridViews[row, col] = tile;
            }
        }
    }
}