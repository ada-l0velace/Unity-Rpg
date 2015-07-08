using UnityEngine;
using System.Collections;

[AddComponentMenu("Grid/Grid Creator")]
public class GridPainter: MonoBehaviour {
    public Color b = Color.black;
    public Color w = new Color(1f, 1f, 1f, 0.5f);

    Material lineMat;

    public float mapWidth = 10;
    public float mapHeight = 10;
    public float spacing = 1f;

    public DrawMode mode = DrawMode.Hexagon;

    void Start() {
        createMat();
        /*Board b = new Board(10,10,30, Orientation.Flat);
        GraphicsEngine gfxe = new GraphicsEngine(b, 0, 0);
        gfxe.Draw();*/
    }


    void OnPostRender() {
        lineMat.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(b);
        switch (mode) {
            case DrawMode.Square: {
                    //horizontal
                    for (int i = 0; i <= mapWidth; i++) {
                        GL.Vertex3(0, 0, i * spacing);
                        GL.Vertex3(mapWidth * spacing, 0, i * spacing);
                    }

                    //vertical
                    for (int i = 0; i <= mapHeight; i++) {
                        GL.Vertex3(i * spacing, 0, 0);
                        GL.Vertex3(i * spacing, 0, mapHeight * spacing);
                    }
                }
                break;
            case DrawMode.Hexagon:
                //http://www.codeproject.com/Articles/14948/Hexagonal-grid-for-games-and-other-projects-Part
                

                break;
        }
        GL.End();
    }

    private void createMat() {
        lineMat = new Material("Shader \"Lines/Colored Blended\" {" +
            "SubShader { Pass { " +
            "    Blend SrcAlpha OneMinusSrcAlpha " +
            "    ZWrite Off Cull Off Fog { Mode Off } " +
            "    BindChannels {" +
            "      Bind \"vertex\", vertex Bind \"color\", color }" +
            "} } }");
        lineMat.hideFlags = HideFlags.HideAndDontSave;
        lineMat.shader.hideFlags = HideFlags.HideAndDontSave;
    }

    public static float CalculateH(float side) {
        return Mathf.Sin(DegreesToRadians(30) * side);
    }

    public static float CalculateR(float side) {
        return Mathf.Cos(DegreesToRadians(30) * side);
    }
    public static float DegreesToRadians(float degrees) {
        return degrees * Mathf.PI / 180f;
    }
}

public enum DrawMode {
    None = 0,
    Square,
    Hexagon
}