using DG.Tweening;
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
    Tween SwapTiles(int row, int col, int targetRow, int targetCol);
    Tween AnimateSwapBack(TileView a, TileView b, float duration);
    void ClearTile(int row, int col);
    Vector2 GetTilePositionInUI(int newRow, int col);
}