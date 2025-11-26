using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    private int _initialPoolSize;
    private Queue<TileView> _tilePool = new Queue<TileView>();
    private Transform _gridParent;
    private TileView _tilePrefab;

    public void InitPool(TileView tilePrefab, Transform gridParent, BoardConfig boardConfig)
    {
        _initialPoolSize = (boardConfig.Height * boardConfig.Width) * 2;
        _tilePrefab = tilePrefab;
        _gridParent = gridParent;
        
        for (int i = 0; i < _initialPoolSize; i++)
        {
            TileView tile = Instantiate(tilePrefab, gridParent);
            tile.gameObject.SetActive(false);
            _tilePool.Enqueue(tile);
        }
    }
    
    public TileView GetTileFromPool()
    {
        if (_tilePool.Count == 0)
        {
            TileView tile = Instantiate(_tilePrefab, _gridParent);
            tile.gameObject.SetActive(false);
            return tile;
        }

        return _tilePool.Dequeue();
    }
    
    public void ReturnTileToPool(TileView tile)
    {
        if (tile == null)
            return;

        tile.OnSwipe += null;
        tile.SetSprite(null);
        tile.gameObject.SetActive(false);
        _tilePool.Enqueue(tile);
    }
}