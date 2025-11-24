using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    [SerializeField] private Image _image;
    
    private int _col;
    private int _row;

    public void SetPositionInUI(int row, int col)
    {
       _row = row;
       _col = col;
    }

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite; 
    }
}