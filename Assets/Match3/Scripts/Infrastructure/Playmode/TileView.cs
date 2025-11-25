using System;
using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    public Sprite Sprit => _image.sprite;
    public event Action<int, int, Vector2Int> OnSwipe;
    public RectTransform RectTransform => _rectTransform;
    
    [SerializeField] private Image _image;
    [SerializeField] private TileSwipe _tileSwipe;
    [SerializeField] private RectTransform _rectTransform;
    
    private int _col;
    private int _row;
    
    public void Construct(BoardView board, Canvas mainCanvas)
    {
        _tileSwipe.Construct(board, mainCanvas);
        _tileSwipe.OnSwipe += HandleSwipe;
    }

    private void HandleSwipe(Vector2Int direction)
    { 
        OnSwipe?.Invoke(_row, _col, direction);
    }

    public void SetPositionInUI(int row, int col)
    {
       _row = row;
       _col = col;
    }

    public void SetSprite(Sprite sprite) => 
        _image.sprite = sprite;
}