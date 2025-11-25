public interface IBoard : IService
{
    int Width { get; }
    int Height { get; }
    int Get(int row, int col);
    void Set(int row, int col, int value);
    void FillRandom(int tileTypesCount);
}