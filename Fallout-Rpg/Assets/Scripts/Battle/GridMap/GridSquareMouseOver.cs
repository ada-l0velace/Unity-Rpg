using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GridSquareMouseOver: MonoBehaviour {
    //private static bool allowPlayerActions;

    private bool displayGUI = true;

    private bool mouseOnBoard;
    private int _raycastLayer;
    private Vector3 _lastPosition;
    public Transform _selectionCube;
    private Vector2Int _mousePosition;

    private HexGrid _grid;
    private bool _playerAct;
    public BattleControl battleCommand;

    private Vector2Int _firstTile;
    private Vector2Int _targetTile;
    private List<Vector2Int> allowedTiles;

    private Dictionary<string, List<Transform>> _tileDict;
    //public List<Material> tileMaterials;
    public Transform tile;
    public Color redSideTile;
    public Color yellowSideTile;
    public Color blueSideTile;
    public Color redTile;
    public Color orangeTile;
    public Color greenTile;
    public Color purpleTile;
    public Color orangeFadedTile;
    public Color debugSideTile;
    public Vector3 hexRotation;
    public Vector3 hexScale;

    /*private List<Transform> orangeTiles, orangeFadedTiles;
    private List<Transform> greenRangeTiles, purpleRangeTiles;
    private Dictionary<string, Transform> redTiles;
    public Transform redSideTile;
    public Transform yellowSideTile;
    public Transform blueSideTile;
    public Transform redTile;
    public Transform orangeTile;
    public Transform greenTile;
    public Transform purpleTile;
    public Transform orangeFadedTile;*/

    void Awake() {
        _raycastLayer = 1 << LayerMask.NameToLayer("RaycastLayer");
        _mousePosition = new Vector2Int();

        /*orangeTiles = new List<Transform>();
        orangeFadedTiles = new List<Transform>();
        greenRangeTiles = new List<Transform>();
        purpleRangeTiles = new List<Transform>();
        redTiles = new Dictionary<string, Transform>();*/
        _tileDict = new Dictionary<string, List<Transform>>();
        _firstTile = null;
        _targetTile = null;
        mouseOnBoard = false;
        /*GridPainter gp = GetComponent<GridPainter>(); //blocked for Hex, needs workaround */
        _grid = GetComponent<HexGrid>();
        _grid.GeneratePoints();
#if UNITY_EDITOR
        _grid.Draw();
#endif
        _playerAct = true;
        //allowPlayerActions = true;
        init();
    }

    public void init() {
        Debug.Log("Initializing battle mode.");
        battleCommand.init(_grid.width, _grid.height, this, _grid.orientation, _grid.hexes);
        battleCommand.newBattle();
    }


    void Update() {
        setPosition(); //grabs the m_x,m_y position compatible with A*
        if (_playerAct) {   //is player turn
            if (mouseOnBoard) {
                debugAstarTileLink();
                if (_mousePosition.ContainedBy(allowedTiles)) { //within unit range of movement

                    if (battleCommand.canCurrentUnitMove)
                        findpath(); //display move path

                    if (Input.GetMouseButtonDown(0)) {
                        //Debug.Log("Player pressed Tile.");
                        battleCommand.tileClick(_mousePosition);
                    }

                } else
                    massTileClear("orangeTile");

#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyMap.BlockNode)) { //block a position
                    battleCommand.block(_mousePosition);
                }
                if (Input.GetKeyDown(KeyMap.UnblockNode)) { //block a position
                    battleCommand.unblock(_mousePosition);
                }
#endif
            }
        }
        /* hotkey caller */
        if (Input.GetKeyDown(KeyMap.ToggleGUI))
            displayGUI = !displayGUI;

    }

    /// <summary>
    /// Call to display AStarNode.sides visually, debug use only
    /// </summary>
    private void debugAstarTileLink() {
        massTileClear("debugSideTile");
        if (_targetTile.x < _grid.width && _targetTile.y < _grid.height && _targetTile.x >= 0 && _targetTile.y >= 0) {
            AStarNode asn = battleCommand.GetNode(_targetTile);
            if (asn != null) {
                AStarNode[] n = asn.Sides;
                Vector2Int v;
                for (int i = 0; i < n.Length; i++) {
                    v = n[i].Position;
                    hexSpawn(debugSideTile, "debugSideTile", new Vector3(v.x, 0.01f, v.y));
                }
            } else {
                Debug.LogError("Null node detected " + _targetTile);
            }
        }
    }



    public List<Vector2Int> findpath() {
        if (_firstTile != null && _targetTile != null) {
            if (_firstTile.Equals(_targetTile)) {
                massTileClear("orangeTile");
            } else {
                List<Vector2Int> path = battleCommand.determinePath(_firstTile, _targetTile);
                massTileClear("orangeTile");
                for (int i = 1, m = path.Count - 1; i < m; i++) {
                    hexSpawn(orangeTile, "orangeTile", new Vector3(path[i].x, 0.01f, path[i].y));
                }
                hexSpawn(orangeTile, "orangeTile", new Vector3(_targetTile.x, 0.01f, _targetTile.y));
                return path;
            }
        }
        return null;
    }



    private void setPosition() {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, _raycastLayer)) {
            mousePos = hit.point; //world position that mouse pointer is hovering

            mousePos.x = Mathf.Floor(mousePos.x);
            mousePos.z = Mathf.Floor(mousePos.z);
            //battleCommand.DetailTile(mousePos);
            if (mousePos.x % 2f != 0) //hex adjustment
                mousePos.z += 0.5f; //mouse hits when it should not feel like it hit

            if (mousePos != _lastPosition) {
                //Debug.Log(mousePos);
                _lastPosition = _selectionCube.position = mousePos;
                _mousePosition.x = (int)mousePos.x;
                _mousePosition.y = (int)mousePos.z;
            }
            _targetTile = _mousePosition.clone();
            mouseOnBoard = true;
        } else {
            mousePos = new Vector3(-1, -1, -1);
            mouseOnBoard = false;
        }
    }


    public Vector3 block(Vector2Int position) {
        return hexSpawn(redTile, "redTile", position.ToVector3()).position;
    }

    public void unblock(Vector2Int position) {
        Vector3 p = new Vector3(position.x, 0f, position.y);
        Vector3 pi = new Vector3(position.x, 0f, position.y + 0.5f);
        List<Transform> l = _tileDict["redTile"];
        for (int i = l.Count; i > -1; --i) {
            if (i % 2 == 0) {
                if (l[i].position == p) {
                    l[i].Recycle();
                    l.RemoveAt(i);
                    return;
                }
            } else {
                if (l[i].position == pi) {
                    l[i].Recycle();
                    l.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void setCurrentTile(Vector2Int position) {
        _firstTile = position;
        hexSpawn(greenTile, "greenTile", new Vector3(position.x, 0f, position.y));
    }

    public void displayArea(List<Vector2Int> area) {
        massTileClear("orangeFadedTile");
        if (allowedTiles != null)
            allowedTiles.Clear();
        if (area != null) {
            allowedTiles = area;
            Vector2Int p;
            for (int i = 0; i < area.Count; i++) {
                p = area[i];
                //Debug.Log(p);
                hexSpawn(orangeFadedTile, "orangeFadedTile", new Vector3(p.x, 0, p.y));
            }
        }
    }
    public void squareSpawn(Transform obj, List<Transform> container, Vector3 v3) {
        container.Add(obj.Spawn(v3)); //new Vector3(path[i].m_x + 0.5f, 0, path[i].m_y + 0.5f)
    }

    public Transform hexSpawn(Color obj, string name, Vector3 v3) {
        List<Transform> container;
        if (_tileDict.ContainsKey(name))
            container = _tileDict[name];
        else
            _tileDict.Add(name, container = new List<Transform>());
        if (_grid.orientation == CellShape.Flat) {
            if (v3.x % 2 != 0)
                v3.z += 0.5f;
        } else if (_grid.orientation == CellShape.Pointy) {
            if (v3.z % 2 != 0)
                v3.x += 0.5f;
        }
        Transform t = tile.Spawn(v3);
        t.GetComponent<Renderer>().material.color = obj;
        t.eulerAngles = hexRotation;
        t.localScale = hexScale;
        t.GetComponentInChildren<Text>().text = "(" + Mathf.Floor(v3.x) + ", " + v3.z + ")";
        container.Add(t);
        return t;
    }

    public void displayUnitsInRange(List<Unit> units) {
        massTileClear("greenRangeTile");
        massTileClear("purpleRangeTile");
        if (units != null) {
            Unit u;
            Vector3 v3 = new Vector3();
            Vector2Int v2;
            for (int i = units.Count - 1; i >= 0; --i) {
                u = units[i];
                v2 = u.node.Position;
                v3.x = v2.x;
                v3.z = v2.y;
                if (u.PlayerControlable) {
                    hexSpawn(greenTile, "greenRangeTile", v3);
                    //greenRangeTiles.Add(greenTile.Spawn(v3));
                } else {
                    hexSpawn(purpleTile, "purpleRangeTile", v3);
                    //orangeRangeTiles.Add(orangeTile.Spawn(v3));
                }
            }
        }
    }

    private void massTileClear(string name) {
        if (_tileDict.ContainsKey(name)) {
            List<Transform> tiles = _tileDict[name];
            if (tiles.Count != 0) {
                for (int i = 0; i < tiles.Count; i++)
                    tiles[i].Recycle();
                tiles.Clear();
            }
        }
    }

    void OnGUI() {
        if (displayGUI) {
            GUI.color = Color.black;
            GUI.Label(new Rect(0f, Screen.height * .5f, 400f, Screen.height * .5f),
                "Mouse Position (World): " + _mousePosition.ToString() +
                "\nMouse Position (Monitor): " + Input.mousePosition.ToString() +
                "\nFirst Tile: " + _firstTile +
                "\nTarget Tile: " + _targetTile

           );

            /*string s = "count: " + allowedTiles.count;
            for (int i = 0; i < allowedTiles.count; i++) {
                s += "\n" + allowedTiles[i].ToString();
            }
            GUI.Label(new Rect(0, Screen.height * .5f, 400, Screen.height * .5f), s);*/

            //Debug.Log(firstTile.ToString() + "" + targetTile.ToString());
        }
    }

    public void playerLock(string reason) {

        _playerAct = false;
        //Debug.Log("Player Locked. " + reason);
    }

    public void playerUnlock(string reason) {
        _playerAct = true;
        //Debug.Log("Player Unlocked." + reason);
    }


    internal void clearAll() {
        massTileClear("orangeFadedTile");
        massTileClear("orangeTile");
        massTileClear("purpleTile");
        massTileClear("greenTile");
    }

}//end of class