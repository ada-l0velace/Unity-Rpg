using System.Collections;
using System.Collections.Generic;

public interface IBattleControlAI
{
    //void tileClick(Vector2Int position);
    bool tileClick(List<Vector2Int> path);
    void endTurn(string reason);
    void aiActions(List<Action> actions);
}
