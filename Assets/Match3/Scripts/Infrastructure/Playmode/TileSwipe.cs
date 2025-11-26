using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSwipe : MonoBehaviour, IDragable, IPointerDownHandler, IPointerUpHandler
{
    public event Action<Vector2Int> OnSwipe; 
    
    [SerializeField] private RectTransform _rectTransform;
    
    private Canvas _canvas;
    private Vector2 _pointerDownPos;
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
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out _pointerDownPos
        );
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 pointerUpPos
        );

        Vector2 diff = pointerUpPos - _pointerDownPos;
        
        if (diff.magnitude < _maxSwipeDistance * 0.3f) 
            return;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            OnSwipe?.Invoke(diff.x > 0 ? Vector2Int.right : Vector2Int.left);
        else
            OnSwipe?.Invoke(diff.y > 0 ? Vector2Int.up : Vector2Int.down);
    }
}