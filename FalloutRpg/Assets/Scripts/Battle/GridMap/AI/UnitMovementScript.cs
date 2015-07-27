using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitMovementScript: MonoBehaviour {

    private Transform _t;
    private List<Vector2Int> _path;
    static private float _speed = 0.1f;

    private Vector3 _helperV3;
    private Vector3 _previousPos;
    private float _incX;
    private float _incZ;
    private IUnitMovement _u;

    void Awake() {
        _t = transform;
        _helperV3 = new Vector3(0f, 0.5f, 0f);
        enabled = false;
        _u = GetComponent<Unit>() as IUnitMovement;
        Debug.Log("moveTo is shit --- not taking into consideration diagonal movement");
    }

    void Update() {
        if (moveTo(_helperV3))
            initMove(true);
    }

    public void begin(List<Vector2Int> path) {
        _path = path;
        initMove(false);
    }

    private void initMove(bool popFirst) {
        _path.RemoveAt(0);
        if (popFirst) {
            if (_path.Count == 0) {
                Vector3 pos = new Vector3(_helperV3.x, _helperV3.y, _helperV3.z);
                _t.position = pos;
                enabled = false;
                _u.movementDone();
                return;
            }
        }

        Vector2Int p = _path[0];
        if (p.x % 2 != 0)
            _helperV3.z = p.y + 0.5f;
        else
            _helperV3.z = p.y;
        _helperV3.x = p.x;

        _previousPos = _t.position;

        if (_helperV3.x != _previousPos.x) {
            if (_helperV3.x > _previousPos.x)
                _incX = 1f * _speed;
            else
                _incX = -1f * _speed;
        } else
            _incX = 0f;

        if (_helperV3.z != _previousPos.z) {
            if (_helperV3.z > _previousPos.z)
                _incZ = 1f * _speed;
            else
                _incZ = -1f * _speed;
        } else
            _incZ = 0f;

        enabled = true;
    }

    private bool moveTo(Vector3 position) {
        bool reached = false;
        
        
        _t.position += (_helperV3 - _t.position) * _speed;
        if (Vector3.Distance(_t.position, _helperV3) < 0.1f)
            reached = true;
        
        return reached;
    }


}