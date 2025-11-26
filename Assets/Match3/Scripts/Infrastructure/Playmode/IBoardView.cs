using UnityEngine;

public interface IBoardView : IService
{
    TileView[,] GetTiles();
    float TileSize { get; }
    float Spacing { get; }
    int TileTypesCount { get; }
    Sprite GetSprite(int tileId);
    void CreateGrid(IBoard board, BoardConfig boardConfig);
    TileView SpawnTile(int row, int col, int tileId);
    void GenerateGridView(IBoard board);
    void SetTilePosition(IBoard board, TileView tile, int row, int col);
    void SwapTiles(int row, int col, int targetRow, int targetCol);
    void ClearTile(int row, int col);
}