using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Not coded for pathfinding yet
/// Using simple go to technic
/// Future Changes:
/// Use of Polyline script and follow a specific path
/// Use of rotation to focus on certain places at certain points
/// Jump from pointo to point
/// </summary>
public class CinematicCamera : MonoBehaviour {

    private static CinematicCamera instance;
    private Dictionary<string, Vector3> positionDict = new Dictionary<string, Vector3>();
    private string m_movingToName = "";
    private Vector3 m_movingTo;
    private Transform m_transform;
    public Vector3 startingPosition;
    public float speed;
    public float distanceToPointThreshold;


    void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
        m_transform = transform;
        MoveToNewPosition("Starting Position", startingPosition);
    }

    private void MoveToNewPosition(string name, Vector3 position) {
        AddPosition(name, position);
        MoveTo(name);
    }


    internal static void AddPosition(string name, Vector3 position) {
        Dictionary<string, Vector3> dict = instance.positionDict;
        #region UNITY_DEBUG
        if (dict.ContainsKey(name)) {
            Debug.LogError("Cinematic Camera already contains the key: " + name);
            return;
        }
        #endregion

        dict.Add(name, position);
    }

    internal static void MoveTo(string name) {
        Dictionary<string, Vector3> dict = instance.positionDict;
        #region UNITY_DEBUG
        if (!dict.ContainsKey(name)) {
            Debug.LogError("Cinematic Camera does not contain the key: " + name);
            return;
        }
        #endregion

        instance.m_movingToName = name;
        instance.m_movingTo = dict[name];
        if (!instance.enabled)
            instance.enabled = true;
    }

    void Update() {
        Vector3 v3 = (m_movingTo - m_transform.position).normalized;

        v3 = m_transform.position += v3 * speed * Time.deltaTime;

        if ((v3 - m_movingTo).magnitude < distanceToPointThreshold) {
            m_transform.position = m_movingTo;
            enabled = false;
        }

    }

} //class