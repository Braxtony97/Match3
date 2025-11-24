using UnityEngine;
using UnityEngine.EventSystems;

public class TileSwipe : MonoBehaviour, IDragable, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _rectTransform;
    
    private Canvas _canvas;
    private bool _isDragging;
    
    private float _maxSwipeDistance;
    private Vector2 _startPosition;

    public void Construct(BoardView board, Canvas parentCanvas)
    {
        _canvas = parentCanvas;
        _maxSwipeDistance = board.TileSize + board.Spacing;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _startPosition = _rectTransform.anchoredPosition;
        _isDragging = true;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;
        
        Vector2 delta = eventData.delta / _canvas.scaleFactor;
        Vector2 targetPos = _rectTransform.anchoredPosition + delta;
        
        targetPos.x = Mathf.Clamp(targetPos.x, _startPosition.x - _maxSwipeDistance, _startPosition.x + _maxSwipeDistance);
        targetPos.y = Mathf.Clamp(targetPos.y, _startPosition.y - _maxSwipeDistance, _startPosition.y + _maxSwipeDistance);
        
        _rectTransform.anchoredPosition = targetPos;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }
}