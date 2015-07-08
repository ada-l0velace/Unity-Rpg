using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/* AI Code name Eva
 * 
 * This AI is not allowed to make any direct changes to any variable
 * It'll only decide how the unit should act
 * 
 */

public class AIControl {
    private Dictionary<Faction, List<Unit>> _faction;
    private List<Unit> _turnOrder;
    private int _currentTurn;
    private Unit _currentUnit;
    private IAStarAI _astar;
    private IBattleControlAI _command;

    private List<Action> actionList;

    public AIControl() { }

    public void init(
        Dictionary<Faction, List<Unit>> faction, List<Unit> turnOrder, 
        IAStarAI astar, IBattleControlAI battleCommand) {
        _faction = faction;
        _turnOrder = turnOrder;
        _astar = astar;
        _command = battleCommand;
        actionList = new List<Action>();
    }

    public void decide(Unit unit, int currentTurn) {
        _currentTurn = currentTurn;
        _currentUnit = unit;

        if (actionList.Count > 0) {
            for (int i = 0; i < actionList.Count; i++)
                actionList[i].dispose();
            actionList.Clear();
        }

        List<Unit> u = _astar.fetchUnitsByArea(unit.node, unit.MovementRate + 1);
        //if there are units in range
        if (u.Count > 0) {

            //get rid of friendly units
            for (int i = u.Count - 1; i >= 0; i--) {
                if (u[i].Faction == unit.Faction) {
                    u.RemoveAt(i);
                }
            }
            if (u.Count > 0) {
                //order by strenght vs most damage dealt

                //move to range of first in pile
                List<Vector2Int> path = _astar.determinePathByPositions(unit.node, u[0].node);
                if (path != null && path.Count - 2 > 0) {
                    //path.RemoveAt(0);
                    path.RemoveAt(path.Count - 1);
                    actionList.Add(new Action(BattleActions.Move, path));
                }
                //attack it
                if (actionList.Count > 0)
                    _command.aiActions(actionList);
                else
                    _command.endTurn("AI close, no action performed");
                return;
            }
        }
        //if there are no units in range
        //find weakest target, move after it
        for (int i = _turnOrder.Count - 1; i >= 0; i--) {
            if (_turnOrder[i].Faction != unit.Faction) {
                List<Vector2Int> path = _astar.determinePathByPositions(unit.node, _turnOrder[i].node);
                path.RemoveAt(0);
                path.RemoveAt(path.Count - 1);
                path.RemoveRange(unit.MovementRate, path.Count - unit.MovementRate);
                actionList.Add(new Action(BattleActions.Move, path));
                _command.aiActions(actionList);
                return;
            }
        }
        _command.endTurn("AI reached end of decide function");



    }


}
