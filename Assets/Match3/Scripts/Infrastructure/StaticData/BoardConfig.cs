using UnityEngine;

[CreateAssetMenu(fileName = "BoardConfig", menuName = "Game/Board Config")]
public class BoardConfig : ScriptableObject, IService
{
    public int Width = 7;
    public int Height = 6;
    public int UniqueTiles = 3;     
}