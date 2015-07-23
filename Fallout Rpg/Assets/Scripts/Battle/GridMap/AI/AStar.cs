using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar: IAStarAI {


    private List<AStarNode> _openList;
    private List<AStarNode> _closedList;
    private AStarNode _checkingNode;
    private AStarNode _firstNode;
    private AStarNode _targetNode;

    public bool foundTarget = false;
    public bool impossiblePath = false;
    public int baseMovementValue = 10;

    private AStarNode[,] map;
    private int _mapWidth;
    private int _mapHeight;
    private CellShape _cellShape;

    /// <summary>
    /// Construtor for pathfinding system, compatible with square and hex(pointy and flat) based grids
    /// </summary>
    /// <param name="mapHeight">Amount of cells from bottom to top</param>
    /// <param name="mapWidth">Amount of cells from left to right</param>
    /// <param name="cellShape">Shape of the cell</param>
    /// <param name="map">Use a pre-created file map</param>
    public AStar(int mapHeight, int mapWidth, CellShape cellShape, AStarNode[,] map = null) {
        _mapHeight = mapHeight;
        _mapWidth = mapWidth;
        _openList = new List<AStarNode>();
        _closedList = new List<AStarNode>();
        _cellShape = cellShape;
        if (cellShape == CellShape.Square) {
            this.map = new AStarNode[_mapHeight, _mapWidth];
            for (int i = 0; i < _mapHeight; i++) {
                for (int j = 0; j < _mapWidth; j++) {
                    this.map[i, j] = new AStarNode(new Vector2Int(i, j), 4, true);
                }
            }
            generateMap();
        } else {
            if (map == null) {
                throw new System.Exception("Map can not be null in current m_orientation.");
            }
            this.map = map;
            generateMap();
        }

    }

    /// <summary>
    /// Builds the A* links cell by cell
    /// Could probably use optimizing
    /// </summary>
    public void generateMap() {
        //int c = 0; //why was this here?
        AStarNode n;


        List<AStarNode> sides = new List<AStarNode>();
        switch (_cellShape) {
            case CellShape.Square:
                #region square
                for (int i = 0; i < _mapWidth; i++) {
                    for (int j = 0; j < _mapHeight; j++) {
                        n = this.map[i, j];
                        if (i + 1 < _mapWidth) //east
                            sides.Add(this.map[i + 1, j]);

                        if (j + 1 < _mapHeight)//south
                            sides.Add(this.map[i, j + 1]);

                        if (i - 1 >= 0)//west
                            sides.Add(this.map[i - 1, j]);

                        if (j - 1 >= 0)//north
                            sides.Add(this.map[i, j - 1]);

                        n.Sides = sides.ToArray();
                        sides.Clear();
                    }
                }
                #endregion
                break;

            case CellShape.Flat:
                #region flat
                for (int i = 0; i < _mapWidth; i++) {
                    for (int j = 0; j < _mapHeight; j++) {
                        n = this.map[i, j];

                        if (i + 1 < _mapWidth) //north east
                            sides.Add(this.map[i + 1, j]);

                        if (j + 1 < _mapHeight) //north
                            sides.Add(this.map[i, j + 1]);

                        if (i - 1 >= 0) //north west
                            sides.Add(this.map[i - 1, j]);

                        if (j - 1 >= 0) //south
                            sides.Add(this.map[i, j - 1]);

                        if (i % 2 == 0) {
                            if ((i + 1 < _mapWidth) && (j - 1 >= 0)) //south east
                                sides.Add(this.map[i + 1, j - 1]);

                            if ((i - 1 >= 0) && (j - 1 >= 0)) //south west
                                sides.Add(this.map[i - 1, j - 1]);
                        } else { //impar
                            if ((i + 1 < _mapWidth) && (j + 1 < _mapHeight)) //south east
                                sides.Add(this.map[i + 1, j + 1]);

                            if ((i - 1 >= 0) && (j + 1 < _mapHeight)) //south west
                                sides.Add(this.map[i - 1, j + 1]);
                        }

                        n.Sides = sides.ToArray();
                        sides.Clear();
                    }
                }
                #endregion
                break;

            case CellShape.Pointy:
                #region pointy
                for (int i = 0; i < _mapWidth; i++) {
                    for (int j = 0; j < _mapHeight; j++) {
                        n = this.map[i, j];

                        if (j + 1 < _mapHeight) { //north
                            sides.Add(this.map[i, j + 1]);

                            if (j % 2 == 0) {
                                if (i - 1 >= 0)
                                    sides.Add(this.map[i - 1, j + 1]);
                            } else if (i + 1 < _mapWidth)
                                sides.Add(this.map[i + 1, j + 1]);
                        }


                        if (i - 1 >= 0) //W
                            sides.Add(this.map[i - 1, j]);


                        if (j - 1 >= 0) { //south
                            sides.Add(this.map[i, j - 1]);
                            if (j % 2 == 0) {
                                if (i - 1 >= 0)
                                    sides.Add(this.map[i - 1, j - 1]);

                            } else if (i + 1 < _mapWidth)
                                sides.Add(this.map[i + 1, j - 1]);
                        }

                        //E
                        if (i + 1 < _mapWidth)
                            sides.Add(this.map[i + 1, j]);

                        n.Sides = sides.ToArray();
                        sides.Clear();
                    }
                }
                #endregion
                break;
        }

    }


    public List<Vector2Int> determinePathByPositions(AStarNode startNode, AStarNode targetNode) {
        determinePath(startNode, targetNode);
        return trackBackPath();
    }

    public List<Vector2Int> determinePathByPositions(Vector2Int startPos, Vector2Int targetPos) {
        determinePath(map[startPos.x, startPos.y], map[targetPos.x, targetPos.y]);
        return trackBackPath();
    }

    public AStarNode determinePath(AStarNode startNode, AStarNode targetNode) {
        if (_targetNode != null) {
            for (int i = 0; i < _mapWidth; i++)
                for (int j = 0; j < _mapHeight; j++)
                    map[i, j].clear();
        }
        _checkingNode = _firstNode = startNode;
        _targetNode = targetNode;
        foundTarget = impossiblePath = false;
        _openList.Clear();
        _closedList.Clear();
        do {
            findPath();
        } while (!foundTarget);
        return _targetNode;
    }

    public AStarNode determinePath(Vector2Int startPos, Vector2Int targetPos) {
        return determinePath(map[startPos.x, startPos.y], map[targetPos.x, targetPos.y]);
    }

    private void findPath() {
        AStarNode[] sides = _checkingNode.Sides;
        for (int i = 0; i < sides.Length; i++) {
            if (sides[i] != null)
                determineNodeValues(_checkingNode, sides[i]);
        }

        _openList.Remove(_checkingNode);
        _closedList.Add(_checkingNode);

        _checkingNode = getSmallestValueNode();
    }

    private AStarNode getSmallestValueNode() {
        if (_openList.Count == 0) {
            foundTarget = true;
            impossiblePath = true;
            return null;
        }
        AStarNode n = _openList[0];
        AStarNode h;
        for (int i = 1; i < _openList.Count; i++) {
            h = _openList[i];
            if (n.g > h.g)
                n = h;
        }
        return n;
    }

    private void determineNodeValues(AStarNode current, AStarNode testing) {
        if (testing == _targetNode) {
            foundTarget = true;
            _targetNode.Parent = current;
            return;
        }

        if (testing.Allowed == false)
            return;

        if (_closedList.Contains(testing) == false) {
            if (_openList.Contains(testing)) {
                int newCost = current.g + baseMovementValue;

                if (newCost < testing.g) {
                    testing.Parent = current;
                    testing.g = newCost;
                    //testing.calculateTotal();
                }
            } else {
                testing.Parent = current;
                testing.g = current.g + baseMovementValue;
                //testing.calculateTotal();
                _openList.Add(testing);
            }
        }
    }

    public List<Vector2Int> trackBackPath() {
        List<Vector2Int> path = new List<Vector2Int>();
        AStarNode node = _targetNode;
        do {
            path.Add(node.Position);
            node = node.Parent;
        } while (node != null);
        path.Reverse();
        return path;
    }

    public void debugTrackBackPath() {
        List<Vector2Int> path = trackBackPath();
        string p = "";
        for (int i = 0; i < path.Count; i++) {
            p += path[i].ToString() + " ";
        }
        Debug.Log(p);
    }

    public bool block(Vector2Int position) {
        if (map[position.x, position.y].Allowed) {
            map[position.x, position.y].Allowed = false;
            return true;
        }
        return false;
    }

    public void unblock(Vector2Int position) {
        map[position.x, position.y].Allowed = true;
    }

    public AStarNode blockAndGetNode(Vector2Int position) {
        if (map[position.x, position.y].Allowed) {
            map[position.x, position.y].Allowed = false;
            return map[position.x, position.y];
        }
        return null;
    }

    public void giveUnit(Vector2Int position, Unit unit) {
        if (map.GetLength(0) > position.x && map.GetLength(1) > position.y) {
            if (map[position.x, position.y].Allowed) {
                map[position.x, position.y].Unit = unit;
            }
        }
    }

    public bool isNodeOpen(Vector2Int position) {
        return map[position.x, position.y].Allowed;
    }

    public List<Vector2Int> fetchOpenPositionsByArea(Vector2Int position, int radius, bool includeStart) {
        return fetchOpenPositionsByArea(map[position.x, position.y], radius, includeStart);
    }

    public List<Vector2Int> fetchOpenPositionsByArea(AStarNode node, int radius, bool includeStart) {
        clearTempData();
        _openList.Add(node);
        int nodeRadius = 0;
        List<Vector2Int> area = new List<Vector2Int>();
        if (includeStart)
            area.Add(_openList[0].Position);
        do {
            _checkingNode = _openList[0];
            _openList.Remove(_checkingNode);
            _closedList.Add(_checkingNode);
            nodeRadius = ++_checkingNode.h;
            if (nodeRadius <= radius) {
                AStarNode[] sides = _checkingNode.Sides;
                for (int i = 0; i < sides.Length; i++) {
                    if (openCheckWithRadius(sides[i], nodeRadius)) {
                        area.Add(sides[i].Position);
                    }
                }
            }
        } while (_openList.Count != 0);

        if (area.Count > 0)
            return area;
        else
            return null;
    }

    private bool openCheckWithRadius(AStarNode n, int nodeRadius) {
        if (n != null) {
            if (!_openList.Contains(n) && !_closedList.Contains(n)) {
                if (n.Allowed) {
                    n.h = nodeRadius;
                    _openList.Add(n);
                    return true;
                } else
                    _closedList.Add(n);
            }
        }
        return false;
    }

    public List<Unit> fetchUnitsByArea(Vector2Int position, int radius) {
        return fetchUnitsByArea(map[position.x, position.y], radius);
    }

    public List<Unit> fetchUnitsByArea(AStarNode node, int radius) {
        clearTempData();
        _openList.Add(node);
        int nodeRadius = 0;
        List<Unit> units = new List<Unit>();
        do {
            _checkingNode = _openList[0];
            _openList.Remove(_checkingNode);
            _closedList.Add(_checkingNode);
            nodeRadius = ++_checkingNode.h;
            if (nodeRadius <= radius) {
                AStarNode[] sides = _checkingNode.Sides;
                AStarNode side;
                for (int i = 0; i < sides.Length; i++) {
                    openCheckWithRadius(sides[i], nodeRadius);
                    side = sides[i];
                    if (side != null && side.Unit != null && !units.Contains(side.Unit))
                        units.Add(side.Unit);
                }
            }
        } while (_openList.Count != 0);

        if (units.Count > 0)
            return units;
        else
            return null;
    }

    private void clearTempData() {
        for (int i = 0; i < _mapWidth; i++)
            for (int j = 0; j < _mapHeight; j++)
                map[i, j].clear();
        _openList.Clear();
        _closedList.Clear();
    }

    public string mapToString() {
        string s = "";
        AStarNode n;
        for (int i = _mapWidth - 1; i >= 0; i--) {
            for (int j = 0; j < _mapHeight; j++) {
                n = map[j, i];
                if (n.Allowed)
                    s += "o";
                else
                    s += "x";
            }
            s += "\n";
        }
        return s;
    }

    internal AStarNode GetNode(Vector2Int pos) {
        try {
            return map[pos.x, pos.y];
        } catch {
            return null;
        }
    }
}

/// <summary>
/// Defines the shape of the cell in the A* system
/// </summary>
public enum CellShape {
    /// <summary>
    /// Regular square
    /// </summary>
    Square = 0,
    /// <summary>
    /// Hex positioned with flat top and bottom
    /// </summary>
    Flat = 1,
    /// <summary>
    /// Hex positioned with m_points at top and botom
    /// </summary>
    Pointy = 2
}