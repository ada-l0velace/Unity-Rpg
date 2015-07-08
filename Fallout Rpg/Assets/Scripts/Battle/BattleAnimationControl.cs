using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleAnimationControl: MonoBehaviour {

    private Transform _t;
    private BattleActions _ba;
    private List<Vector2Int> _path;
    private float _speed = 0.1f;

    public delegate void CallBack();
    private CallBack _func;

    private Vector3 _helperV3;
    private Vector3 _previousPos;
    private float _incX;
    private float _incZ;
    private bool _acting;

    void Start() {
        enabled = false;
        _acting = false;
        _helperV3 = new Vector3(0f, 0.5f, 0f);
    }

    void Update() {
        //Debug.Log("Animation Ran.");
        //Vector3 v = new Vector3(_path[_path.count - 1].m_x + 0.5f, 0.5f, _path[_path.count - 1].m_y + 0.5f);
        //_t.position = v;
        if (_acting) {
            switch (_ba) {
                case BattleActions.Move:
                    if (moveTo(_helperV3)) {
                        _acting = false;
                        initMove(true);
                    }
                    break;
                default:
                    Debug.LogError("Attempting to run an unknown animation.");
                    break;
            }
        }
        //enabled = false;
        //_func();
    }

    public void runAnimation(
        Transform target, BattleActions action, List<Vector2Int> path, CallBack function
      ) {
        //Debug.Log("Animation requested.");
        _t = target;
        _ba = action;
        _path = path;
        _func = function;
        enabled = true;
        switch (action) {
            case BattleActions.Move:
                initMove(false);
                break;
            default:
                Debug.LogError("Attempting to START running an unknown animation.");
                break;
        }
    }

    private void initMove(bool popFirst) {
        if (_acting)
            Debug.Log("OY, I'm working ovah here!");

        if (popFirst) {
            _path.RemoveAt(0);
            if (_path.Count == 0) {
                _func();
                return;
            }
        }

        _acting = true;

        Vector2Int p = _path[0];
        _helperV3.x = p.x + 0.5f;
        _helperV3.z = p.y + 0.5f;
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
    }

    private bool moveTo(Vector3 position) {
        bool reached = false;
        _previousPos.x += _incX;
        _previousPos.z += _incZ;


        if ((_incX > 0 && _previousPos.x >= position.x) ||
            (_incX < 0 && _previousPos.x <= position.x)) {
            _previousPos.x = position.x;
            reached = true;
        } else if (_incX == 0f)
            reached = true;

        if ((_incZ > 0 && _previousPos.z >= position.z) ||
            (_incZ < 0 && _previousPos.z <= position.z)) {
            _previousPos.z = position.z;
            reached = true;
        } else if (_incZ == 0f)
            reached &= true;

        //Debug.Log("Moving unit. Reached:" + reached + " incX: " + _incX + " incZ: " + _incZ);
        _t.position = _previousPos;
        return reached;
    }

}
