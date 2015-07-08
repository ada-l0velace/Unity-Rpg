using UnityEngine;
using System.Collections;

public class AStarNode {
    private static int ID_COUNTER = -1;
    private int _id;
    private Vector2Int _pos;
    private bool _allowed;
    private Unit _unit;
    private TileType type;

    public int h; //heuristics value
    public int g; //movement value
    public int f; //total cost
    private AStarNode _parent;
    private AStarNode[] _sides;

    
    /*private AStarNode _north;
    private AStarNode _east;
    private AStarNode _south;
    private AStarNode _west;*/

    //m_h and f not actually being used.

    public AStarNode(Vector2Int position, int sides, bool allowed=true) {
        _allowed = allowed;
        _id = ++ID_COUNTER;
        _pos = position;
        _sides = new AStarNode[sides];
    }


    #region Accessors
    /*public AStarNode North {
        get { return _north; }
        set { _north = value; }
    }
    public AStarNode East {
        get { return _east; }
        set { _east = value; }
    }
    public AStarNode South {
        get { return _south; }
        set { _south = value; }
    }
    public AStarNode West {
        get { return _west; }
        set { _west = value; }
    }*/

    public AStarNode[] Sides {
        get { return _sides; }
        set { _sides = value; }
    }

    public AStarNode Parent {
        get { return _parent; }
        set { _parent = value; }
    }
    public bool Allowed {
        get { return _allowed; }
        set { _allowed = value; }
    }
    public int ID {
        get { return _id; }
    }
    public virtual Vector2Int Position {
        get { return _pos.clone(); }
    }

    public Unit Unit {
        get {
            return _unit;
        }
        set {
            if (_allowed && _unit == null) {
                _unit = value;
                _unit.node = this;
                _allowed = false;
            } else
                Debug.LogError("Failed to give unit to Node " + _id +
                    " Allowed:" + _allowed + " Has unit: " + (_unit != null));
        }
    }
    public Unit FetchUnit {
        get {
            _allowed = true;
            Unit u = _unit;
            _unit = null;
            u.node = null;
            return u;
        }
    }
    #endregion

    public void calculateTotal() {
        f = g + h;
    }

    public void clear() {
        f = g = h = 0;
        _parent = null;
    }

    public void Dispose() {
        _pos = null;
        _parent = null;
        /*_north = null;
        _east = null;
        _south = null;
        _west = null;*/
    }

    public Vector2Int[] getInvertedPathPosition() {
        int c = 1;
        AStarNode node = this;
        do {
            node = node.Parent;
            ++c;
        } while (node != null);
        if (c > 1) {
            Vector2Int[] path = new Vector2Int[--c];
            node = this;
            path[--c] = Position;
            do {
                node = node.Parent;
                if (node != null)
                    path[--c] = node.Position;
            } while (node != null);
            return path;
        }
        return null;
    }

    public string SidesString {
        get {
            string s = "";
            for (int i = 0; i < _sides.Length; i++) {
                s += _sides[i].Position + " ";
            }
            return s;
        }
    }
}

public enum TileType {
    Dirt,
    Grass,
    Forest,
    Mountain
}