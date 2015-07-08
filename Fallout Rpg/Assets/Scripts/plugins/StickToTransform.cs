using UnityEngine;
using System.Collections;

public class StickToTransform: MonoBehaviour {

    public Transform stickTo;
    public bool lockYAxis = true;
    public bool lockXAxis = true;
    public bool lockZAxis = true;
    public float stickToLerp;

    private Transform m_transform;
    [HideInInspector]
    public Vector3 offset;
    private float startingY;
    private float startingX;
    private float startingZ;


    void Start() {
        Debug.LogError("Makes things shake a lot.");
        m_transform = transform;
        offset = m_transform.position - stickTo.position;
        startingY = m_transform.position.y;
    }


    void Update() {
        Vector3 p = Vector3.Lerp(m_transform.position, stickTo.position + offset, Time.deltaTime * stickToLerp);
        if (lockYAxis)
            p.y = startingY;

        if (lockXAxis)
            p.x = startingX;

        if (lockZAxis)
            p.z = startingZ;
        m_transform.position = p;
    }

}
