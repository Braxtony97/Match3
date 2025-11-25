using System.Collections.Generic;
using UnityEngine;

public class MatchResult
{
    public HashSet<Vector2Int> MatchedTiles = new HashSet<Vector2Int>();
    public bool HasMatches => MatchedTiles.Count > 0;   
}