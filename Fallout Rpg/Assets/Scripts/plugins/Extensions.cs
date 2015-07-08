using UnityEngine;
using System.Collections;


public static class RectExtension {

    public static Rect RectSumVector2(this Rect rect, Vector2 vector) {
        rect.x += vector.x;
        rect.y += vector.y;
        return rect;
    }

    public static SimpleRect RectSumVector2(this SimpleRect rect, Vector2 vector) {
        rect.x += vector.x;
        rect.y += vector.y;
        return rect;
    }

    public static Rect Offset(this Rect rect, Rect other) {
        return new Rect(other.x + rect.x, other.y + rect.y, rect.width, rect.height);
    }

}

public struct SimpleRect {
    public float x;
    public float y;
    public float width;
    public float height;

    /*public SimpleRect() {
        this.x = 0;
        this.y = 0;
        this.width = 0;
        this.height = 0;
    }
    */
    public SimpleRect(float x, float y, float width, float height) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public SimpleRect(SimpleRect rect) {
        x = rect.x;
        y = rect.y;
        width = rect.width;
        height = rect.height;
    }

    public SimpleRect(Rect rect) {
        x = rect.x;
        y = rect.y;
        width = rect.width;
        height = rect.height;
    }

    public override string ToString() {
        return "{x:" + x + ", y:" + y + ", width:" + width + ", height:" + height + "}";
    }
}