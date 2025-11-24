using UnityEngine;

public class TileView : MonoBehaviour
{
    private int _col;
    private int _row;

    public void SetPositionInUI(int row, int col)
    {
       _row = row;
       _col = col;
    }

    public void SetSprite(int i)
    {
    }
}