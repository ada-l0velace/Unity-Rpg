
using UnityEngine;
using System.Collections;

public class GUIUtilities {

    #region Progress Bar
    public static void DrawProgressBar(float currentValue, float maxValue, GUINumberDisplay display,
        Rect bar, Rect background,
        Texture2D barTexture, Texture2D backgroundTexture) {
        GUI.DrawTexture(background, backgroundTexture);
        DrawProgressBar(currentValue, maxValue, display, bar, barTexture);
    }

    /* public static void DrawProgressBar(float currentValue, float maxValue, GUINumberDisplay display,
         Rect bar, Rect foreground,
         Texture2D barTexture, Texture2D foregroundTexture) {
         // does not compile conflict with background draw
         DrawProgressBar(currentValue, maxValue, display, bar, barTexture);
         GUI.DrawTexture(foreground, foregroundTexture);
     }*/

    public static void DrawProgressBar(float currentValue, float maxValue, GUINumberDisplay display,
        Rect bar, Rect background, Rect foreground,
        Texture2D barTexture, Texture2D backgroundTexture, Texture2D foregroundTexture) {

        GUI.DrawTexture(background, backgroundTexture);
        DrawProgressBar(currentValue, maxValue, display, bar, barTexture);
        GUI.DrawTexture(foreground, foregroundTexture);
    }

    public static void DrawProgressBar(float currentValue, float maxValue, GUINumberDisplay display,
        Rect bar, Texture2D barTexture = null) {

        string content = "";
        if (display == GUINumberDisplay.None) {

        } else if ((GUINumberDisplay.CurrentAndMax & GUINumberDisplay.Percentage) == display)
            content = currentValue.ToString("F0") + "/" + maxValue.ToString("F0") + " (" + ((currentValue / maxValue) * 100).ToString("F0") + "%)";
        else if (display == GUINumberDisplay.CurrentAndMax) {
            content = currentValue.ToString("F0") + "/" + maxValue.ToString("F0");
        } else if (display == GUINumberDisplay.CurrentOnly) {
            content = currentValue.ToString("F0");
        } else if (display == GUINumberDisplay.Percentage) {
            content = ((currentValue / maxValue) * 100).ToString("F0") + "%";
        } else {
            throw new System.Exception("No valid display was given for progress bar.");
        }

        float w = bar.width;
        bar.width *= currentValue / maxValue;
        if (barTexture != null)
            GUI.DrawTexture(bar, barTexture);
        else
            GUI.Box(bar, content);

        if (content != "") {
            bar.width = w;
            GUI.Label(bar, content);
        }
    }
    #endregion


    #region math stuff

    public static Rect Sum(Rect r1, Rect r2) {
        return new Rect(r1.x + r2.x, r1.y + r2.y, r1.width, r1.height);
    }

    #endregion



} //class

public enum GUINumberDisplay {
    None = 0,
    CurrentOnly = (1 << 1),
    CurrentAndMax = (1 << 2),
    Percentage = (1 << 3)
}


public class Drawing {
    //****************************************************************************************************
    //  static function DrawLine(rect : Rect) : void
    //  static function DrawLine(rect : Rect, color : Color) : void
    //  static function DrawLine(rect : Rect, width : float) : void
    //  static function DrawLine(rect : Rect, color : Color, width : float) : void
    //  static function DrawLine(Vector2 pointA, Vector2 pointB) : void
    //  static function DrawLine(Vector2 pointA, Vector2 pointB, color : Color) : void
    //  static function DrawLine(Vector2 pointA, Vector2 pointB, width : float) : void
    //  static function DrawLine(Vector2 pointA, Vector2 pointB, color : Color, width : float) : void
    //  
    //  Draws a GUI line on the screen.
    //  
    //  DrawLine makes up for the severe lack of 2D line rendering in the Unity runtime GUI system.
    //  This function works by drawing a 1x1 texture filled with a color, which is then scaled
    //   and rotated by altering the GUI matrix.  The matrix is restored afterwards.
    //****************************************************************************************************

    public static Texture2D lineTex;

    public static void DrawLine(Rect rect) { DrawLine(rect, GUI.contentColor, 1.0f); }
    public static void DrawLine(Rect rect, Color color) { DrawLine(rect, color, 1.0f); }
    public static void DrawLine(Rect rect, float width) { DrawLine(rect, GUI.contentColor, width); }
    public static void DrawLine(Rect rect, Color color, float width) { DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width); }
    public static void DrawLine(Vector2 pointA, Vector2 pointB) { DrawLine(pointA, pointB, GUI.contentColor, 1.0f); }
    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color) { DrawLine(pointA, pointB, color, 1.0f); }
    public static void DrawLine(Vector2 pointA, Vector2 pointB, float width) { DrawLine(pointA, pointB, GUI.contentColor, width); }
    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width) {
        if (pointA == pointB)
            return;
        // Save the current GUI matrix, since we're going to make changes to it.
        Matrix4x4 matrix = GUI.matrix;

        // Generate a single pixel texture if it doesn't exist
        if (!lineTex) { lineTex = new Texture2D(1, 1); }

        // Store current GUI color, so we can switch it back later,
        // and set the GUI color to the color parameter
        Color savedColor = GUI.color;
        GUI.color = color;

        // Determine the angle of the line.
        float angle = Vector3.Angle(pointB - pointA, Vector2.right);

        // Vector3.Angle always returns a positive number.
        // If pointB is above pointA, then angle needs to be negative.
        if (pointA.y > pointB.y) { angle = -angle; }

        // Use ScaleAroundPivot to adjust the size of the line.
        // We could do this when we draw the texture, but by scaling it here we can use
        //  non-integer values for the width and length (such as sub 1 pixel widths).
        // Note that the pivot point is at +.5 from pointA.y, this is so that the width of the line
        //  is centered on the origin at pointA.
        GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));

        // Set the rotation for the line.
        //  The angle was calculated with pointA as the origin.
        GUIUtility.RotateAroundPivot(angle, pointA);

        // Finally, draw the actual line.
        // We're really only drawing a 1x1 texture from pointA.
        // The matrix operations done with ScaleAroundPivot and RotateAroundPivot will make this
        //  render with the proper width, length, and angle.
        GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);

        // We're done.  Restore the GUI matrix and GUI color to whatever they were before.
        GUI.matrix = matrix;
        GUI.color = savedColor;
    }
}