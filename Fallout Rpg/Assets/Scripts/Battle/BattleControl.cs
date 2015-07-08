using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleControl: MonoBehaviour, IBattleControlAI {

    //holds all the units in the battle by faction
    private Dictionary<Faction, List<Unit>> _faction;
    //holds all units based on their turn order
    private List<Unit> _turnOrder;
    private int _turnLenght;
    private int _currentTurn;
    private int _turnNumber;
    private Unit _currentUnit;

    private AStar _astar;
    private AIControl _ai;
    private GridSquareMouseOver _grid;
    //private BattleAnimationControl _bac;

    public Unit FriendlyDummy;
    public Unit EnemyDummy;



    public void init(int mapHeight, int mapWidth, GridSquareMouseOver grid, Orientation orientation, AStarNode[,] nodes = null) {
        _astar = new AStar(mapHeight, mapWidth, orientation, nodes);
        _ai = new AIControl();
        _faction = new Dictionary<Faction, List<Unit>>();
        _grid = grid;
        //_bac = GetComponent<BattleAnimationControl>();
    }

    

    public void newBattle() {
        _turnOrder = new List<Unit>();
        _turnNumber = _currentTurn = 0;
        addUnit(FriendlyDummy, new Vector2Int(0, 0), 12, Faction.Player, true);
        addUnit(EnemyDummy, new Vector2Int(15, 15), 6, Faction.Enemy, false);

        block(new Vector2Int(3, 3));
        block(new Vector2Int(4, 4));
        block(new Vector2Int(5, 4));
        block(new Vector2Int(5, 5));
        block(new Vector2Int(6, 6));
        block(new Vector2Int(7, 6));
        block(new Vector2Int(7, 7));

        _turnLenght = _currentTurn = _turnOrder.Count;
        _ai.init(_faction, _turnOrder, _astar, this);
        nextTurn("Battle starting.");
    }

    private void addUnit(Unit unit, Vector2Int position, int movementRate, Faction factionName, bool playerControlable) {
        Unit u = unit.Spawn();
        if (position.x % 2 != 0)
            u.transform.position = new Vector3(position.x, 0.5f, position.y + 0.5f);
        else
            u.transform.position = new Vector3(position.x, 0.5f, position.y);
        if (!_faction.ContainsKey(factionName))
            _faction[factionName] = new List<Unit>();
        _faction[factionName].Add(u);
        _astar.giveUnit(position, u);
        u.init(movementRate, factionName, playerControlable);
        _turnOrder.Add(u);

        //_grid.block(position);
    }

    private void nextTurn(string reason) {
        ++_turnNumber;
        //Debug.Log("Turn " + _turnNumber + " " + _currentUnit.Faction + ". " + reason);
        if (++_currentTurn >= _turnLenght)
            _currentTurn = 0;
        _grid.displayUnitsInRange(null);
        _currentUnit = _turnOrder[_currentTurn];
        _currentUnit.resetAP();
        _grid.clearAll();
        _grid.setCurrentTile(_currentUnit.node.Position);
        //Debug.Log("---" + _currentUnit.Faction + " unit turn. ---");
        if (_currentUnit.PlayerControlable) {
            _grid.displayArea(_astar.fetchOpenPositionsByArea(
                _currentUnit.node, _currentUnit.MovementRate, false));
            displayUnitsInRange(true);
            _grid.playerUnlock("New turn or player by BattleControl.nextTurn()");
        } else {
            _grid.playerLock("New turn for AI by BattleControl.nextTurn()");
            _ai.decide(_currentUnit, _currentTurn);
        }
    }

    public List<Vector2Int> determinePath(Vector2Int start, Vector2Int target) {
        return _astar.determinePathByPositions(start, target);
    }
    //why does tileclick receive a path?
    //should only receive Vector2Int of tileClicked then get path/attack
    public bool tileClick(Vector2Int click) {
        List<Vector2Int> path = determinePath(_currentUnit.node.Position, click);
        Vector2Int lastPos = path[path.Count - 1];
        if (_astar.isNodeOpen(lastPos)) {
            _astar.giveUnit(lastPos, _currentUnit.node.FetchUnit);
            _currentUnit.runAnimation(BattleActions.Move, path);
            _grid.playerLock("TileClicked.");
            /* BLOCKED FOR ANIMATION CODING
             * CHANGE AFTERWARDS
            p.m_y = _currentUnit.transform.position.m_y;
            _currentUnit.transform.position = p;
            //if unit has no actions avaiable -> nextTurn()
            nextTurn();
             */
            hasUnitsInRange = displayUnitsInRange(false) > 0;
            return true;
        }
        Debug.Log("Node Is Blocked. " + lastPos);
        return false;
    }

    /// <summary>
    /// Use Vector2Int version instead
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    [System.Obsolete]
    public bool tileClick(List<Vector2Int> path) {
        Vector2Int lastPos = path[path.Count - 1];
        if (_astar.isNodeOpen(lastPos)) {
            _astar.giveUnit(lastPos, _currentUnit.node.FetchUnit);
            _currentUnit.runAnimation(BattleActions.Move, path);
            _grid.playerLock("TileClicked.");
            /* BLOCKED FOR ANIMATION CODING
             * CHANGE AFTERWARDS
            p.m_y = _currentUnit.transform.position.m_y;
            _currentUnit.transform.position = p;
            //if unit has no actions avaiable -> nextTurn()
            nextTurn();
             */
            return true;
        }
        Debug.Log("Node Is Blocked. " + lastPos);
        return false;
    }

    public void endTurn(string reason) {
        //Debug.Log("Turn ended: " + reason);
        nextTurn("End of turn. " + reason);
    }

    public void aiActions(List<Action> actions) {
        //THIS IS COMPLETLY FUCKING WRONG FOR THE SAKE OF FASTER TESTING
        //why you fuck?
        if (actions.Count > 0 && actions[0].actionType == BattleActions.Move) {
            tileClick(actions[0].data as List<Vector2Int>);
        }
    }

    private bool hasUnitsInRange;
    void Update() {
        if (_currentUnit.hasMoved) {
            if (_currentUnit.hasAttacked) {
                nextTurn("No more Action Points.");
                return;
            }
            if (_currentUnit.PlayerControlable) {
                if (!hasUnitsInRange)
                    nextTurn("Player out of actions.");
            } else {
                nextTurn("AI passed. Incomplete code.");
            }
        }
    }

    void OnGUI() {
        if (_currentUnit.PlayerControlable)
            if (GUI.Button(new Rect(Screen.width - 75, Screen.height - 50, 75, 50), "Next Turn"))
                nextTurn("Player passed.");
        GUILayout.Box(_currentUnit.Faction + "\nTurn " + _turnNumber);

        GUILayout.Box("Is Player: " + _currentUnit.PlayerControlable);
    }

    private int displayUnitsInRange(bool withMovement) {
        int r = _currentUnit.Range;
        if (withMovement)
            r += _currentUnit.MovementRate;
        List<Unit> u = _astar.fetchUnitsByArea(_currentUnit.node, r);
        if (u != null)
            u.Remove(_currentUnit);
        _grid.displayUnitsInRange(u);
        return (u != null) ? u.Count : 0;
    }


    public void block(Vector2Int position) {
        if (_astar.block(position))
            _grid.block(position);
    }

    public void unblock(Vector2Int position) {
        _astar.unblock(position);
        _grid.unblock(position);
    }
    /*
#if UNITY_EDITOR
    private AStarNode _lastNodeHovered = null;
    private List<Transform> debugTiles = new List<Transform>();
    public Transform debugTile;
    internal void DetailTile(Vector3 mousePos) {

        AStarNode node = _astar.GetNode(new Vector2Int((int)mousePos.m_x, (int)mousePos.z));
        if (node != null && node != _lastNodeHovered) {
            for (int i = 0; i < debugTiles.count; i++) {
                debugTiles[i].Recycle();
            }
            debugTiles.Clear();

            _lastNodeHovered = node;
            Debug.Log(node.Position + " Sides: " + node.SidesString);

            Vector3 p;
            Vector2Int v;
            for (int i = 0; i < node.Sides.Length; i++) {
                v = node.Sides[i].Position;
                p = new Vector3(v.m_x, 0.1f, v.m_y);
                if (v.m_x % 2 != 0)
                    p.z += 0.5f;
                debugTiles.Add(debugTile.Spawn(p));
            }
        } else if (node == null) {
            for (int i = 0; i < debugTiles.count; i++) {
                debugTiles[i].Recycle();
			}
            debugTiles.Clear();
        }
    }
#endif*/

    public bool canCurrentUnitMove {
        get {
            return !_currentUnit.hasMoved;
        }
    }
}