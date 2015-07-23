using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit: MonoBehaviour, IUnitMovement {

    public static int ActionPoints = 2;
    private int _actionPoints;
    private bool _hasAttacked;
    private bool _hasMoved;

    private int _moveRate;
    private bool _playerControlable;
    private Faction _faction;
    private UnitMovementScript _ums;
    private UnitCombatStats _ucs;

    public AStarNode node;

    public delegate void OnMoveAnimationCompletedEvent();
    public OnMoveAnimationCompletedEvent onMoveAnimationCompleted;
    private OnMoveAnimationCompletedEvent removeMeOnNextCall;

    public void init(int movementRate, Faction faction, bool playerControlable) {
        _moveRate = movementRate;
        _playerControlable = playerControlable;
        _faction = faction;
        _ums = GetComponent<UnitMovementScript>();
        _ucs = GetComponent<UnitCombatStats>();
        _hasAttacked = _hasMoved = false;
    }

    #region Accessors
    public int MovementRate {
        get { return _moveRate; }
    }

    public int Range {
        get { return _ucs.Range; }
    }

    public Faction Faction {
        get { return _faction; }
    }

    public bool PlayerControlable {
        get { return _playerControlable; }
    }

    public bool hasMoved {
        get { return _hasMoved; }
    }
    public bool hasAttacked {
        get { return _hasAttacked; }
    }
    #endregion

    //-------------------------------------------------
    //-------------------------------------------------
    //-------------------------------------------------
    //-------------------------------------------------

    private Transform _t;
    private BattleActions _ba;
    private List<Vector2Int> _path;
    static private float _speed = 0.1f;

    private Vector3 _helperV3;
    private Vector3 _previousPos;
    private float _incX;
    private float _incZ;

    void Start() {
        _t = transform;
        _helperV3 = new Vector3(0f, 0.5f, 0f);
        enabled = false;
    }

    public void runAnimation(BattleActions action, List<Vector2Int> path, OnMoveAnimationCompletedEvent func) {
        //Debug.Log("Animation requested.");
        _ba = action;
        _path = path;
        switch (action) {
            case BattleActions.Move:
                onMoveAnimationCompleted += func;
                removeMeOnNextCall += func;
                _ums.begin(_path);
                break;
            default:
                Debug.LogError("Attempting to START running an unknown animation.");
                break;
        }
    }

    public void movementDone() {
        _hasMoved = true;
        onMoveAnimationCompleted();
        onMoveAnimationCompleted -= removeMeOnNextCall;
        removeMeOnNextCall = null;
    }

    public void resetAP() {
        _hasAttacked = _hasMoved = false;
        _actionPoints = ActionPoints;
    }
}