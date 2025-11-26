public class BoardGravityService
{
    private readonly IBoard _board;
    private readonly IBoardView _boardView;

    public BoardGravityService(IBoard board, IBoardView boardView)
    {
        _board = board;
        _boardView = boardView;
    }

    public void CollapseBoard()
    {
        for (int col = 0; col < _board.Width; col++)
        {
            int writeRow = _board.Height - 1;

            for (int row = _board.Height - 1; row >= 0; row--)
            {
                int val = _board.Get(row, col);

                if (val != -1)
                {
                    _board.Set(writeRow, col, val);
                    writeRow--;
                }
            }
            
            for (int row = writeRow; row >= 0; row--)
                _board.Set(row, col, -1);
        }
    }

    public void FillEmptyTiles()
    {
        int types = _boardView.TileTypesCount;

        for (int row = 0; row < _board.Height; row++)
        {
            for (int col = 0; col < _board.Width; col++)
            {
                if (_board.Get(row, col) == -1)
                {
                    int newTile = UnityEngine.Random.Range(0, types);
                    _board.Set(row, col, newTile);
                }
            }
        }
    }
}