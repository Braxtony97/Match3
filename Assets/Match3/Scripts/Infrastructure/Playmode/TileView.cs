using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    public RectTransform RectTransform => _rectTransform;
    
    [SerializeField] private Image _image;
    [SerializeField] private TileSwipe _tileSwipe;
    [SerializeField] private RectTransform _rectTransform;
    
    private int _col;
    private int _row;
    
    public void Construct(BoardView board, Canvas mainCanvas) => 
        _tileSwipe.Construct(board, mainCanvas);

    public void SetPositionInUI(int row, int col)
    {
       _row = row;
       _col = col;
    }

    public void SetSprite(Sprite sprite) => 
        _image.sprite = sprite;
}