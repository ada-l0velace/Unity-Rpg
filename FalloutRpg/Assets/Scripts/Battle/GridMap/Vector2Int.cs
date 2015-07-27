using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vector2Int {

    public int x;
    public int y;

    public Vector2Int() {
        y = x = 0;
    }

    public Vector2Int(int x, int y) {
        this.y = y;
        this.x = x;
    }

    public int sqrMagnitude {
        get { return x * x + y * y; }
    }

    public override string ToString() {
        return "(" + x + "," + y + ")";
    }

    public Vector2Int clone() {
        return new Vector2Int(x, y);
    }
    public bool Equals(Vector2Int value) {
        return x == value.x && y == value.y;
    }

    public bool ContainedBy(List<Vector2Int> list) {
        Vector2Int value;
        if (list != null)
            for (int i = list.Count - 1; i >= 0; i--) {
                value = list[i];
                if (x == value.x && y == value.y)
                    return true;
            }
        return false;
    }

    public Vector3 ToVector3() {
        return new Vector3(x, 0f, y);
    }
}