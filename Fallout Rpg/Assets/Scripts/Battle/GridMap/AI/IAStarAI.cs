using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAStarAI 
{

    List<Vector2Int> determinePathByPositions(AStarNode startNode, AStarNode targetNode);
    List<Vector2Int> determinePathByPositions(Vector2Int startPos, Vector2Int targetPos);

    List<Vector2Int> fetchOpenPositionsByArea(AStarNode node, int radius, bool includeStart);
    List<Vector2Int> fetchOpenPositionsByArea(Vector2Int position, int radius, bool includeStart);

    List<Unit> fetchUnitsByArea(Vector2Int position, int radius);
    List<Unit> fetchUnitsByArea(AStarNode node, int radius);
}
