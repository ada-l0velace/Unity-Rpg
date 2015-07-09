using UnityEngine;
using ProtoTurtle.BitmapDrawing;

public class HexGrid: MonoBehaviour {

    private Vector2[] points;
    private float h;
    private float r;
    [HideInInspector]
    public Hex[,] hexes;
    private float pixelWidth;
    private float pixelHeight;

    public GameObject target;
    public Orientation orientation;
    public int width;
    public int height;
    public float side;
    public float xOffset;
    public float yOffset;
    public Color gridColor;

    /*public void Init() {
        GeneratePoints();
        Draw();
    }*/

    public void GeneratePoints() {
        hexes = new Hex[height, width]; //opposite of what we'd expect

        float h = HexMath.CalculateH(side); // short m_side
        float r = HexMath.CalculateR(side); // long m_side

        //
        // Calculate pixel info..remove?
        // because of staggering, need to add an extra m_r/m_h
        float hexWidth = 0;
        float hexHeight = 0;
        switch (orientation) {
            case Orientation.Flat:
                hexWidth = side + h;
                hexHeight = r + r;
                this.pixelWidth = (width * hexWidth) + h;
                this.pixelHeight = (height * hexHeight) + r;
                break;
            case Orientation.Pointy:
                hexWidth = r + r;
                hexHeight = side + h;
                this.pixelWidth = (width * hexWidth) + r;
                this.pixelHeight = (height * hexHeight) + h;
                break;
            default:
                break;
        }


        bool inTopRow = false;
        bool inBottomRow = false;
        bool inLeftColumn = false;
        bool inRightColumn = false;
        bool isTopLeft = false;
#pragma warning disable
        bool isTopRight = false;
        bool isBotomLeft = false;
        bool isBottomRight = false;
#pragma warning restore

        // i = m_y coordinate (weakRows), j = m_x coordinate (columns) of the hex tiles 2D plane
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                // Set position booleans
                #region Position Booleans
                if (i == 0) {
                    inTopRow = true;
                } else {
                    inTopRow = false;
                }

                if (i == height - 1) {
                    inBottomRow = true;
                } else {
                    inBottomRow = false;
                }

                if (j == 0) {
                    inLeftColumn = true;
                } else {
                    inLeftColumn = false;
                }

                if (j == width - 1) {
                    inRightColumn = true;
                } else {
                    inRightColumn = false;
                }

                if (inTopRow && inLeftColumn) {
                    isTopLeft = true;
                } else {
                    isTopLeft = false;
                }

                if (inTopRow && inRightColumn) {
                    isTopRight = true;
                } else {
                    isTopRight = false;
                }

                if (inBottomRow && inLeftColumn) {
                    isBotomLeft = true;
                } else {
                    isBotomLeft = false;
                }

                if (inBottomRow && inRightColumn) {
                    isBottomRight = true;
                } else {
                    isBottomRight = false;
                }
                #endregion

                //
                // Calculate Hex positions
                //
                if (isTopLeft) {
                    //First hex
                    switch (orientation) {
                        case Orientation.Flat:
                            hexes[0, 0] = new Hex(0 + h + xOffset, 0 + yOffset, side, orientation, new Vector2Int(0,0));
                            break;
                        case Orientation.Pointy:
                            hexes[0, 0] = new Hex(0 + r + xOffset, 0 + yOffset, side, orientation, new Vector2Int(0, 0));
                            break;
                        default:
                            break;
                    }



                } else {
                    switch (orientation) {
                        case Orientation.Flat:
                            if (inLeftColumn) {
                                // Calculate from hex above
                                hexes[i, j] = new Hex(hexes[i - 1, j].Points[(int)FlatVertice.BottomLeft], side, orientation, new Vector2Int(i, j));
                            } else {
                                // Calculate from Hex to the left and need to stagger the columns
                                if (j % 2 == 0) {
                                    // Calculate from Hex to left's Upper Right Vertice plus m_h and R offset 
                                    float x = hexes[i, j - 1].Points[(int)FlatVertice.UpperRight].x;
                                    float y = hexes[i, j - 1].Points[(int)FlatVertice.UpperRight].y;
                                    x += h;
                                    y -= r;
                                    hexes[i, j] = new Hex(x, y, side, orientation, new Vector2Int(i, j));
                                } else {
                                    // Calculate from Hex to left's Middle Right Vertice
                                    hexes[i, j] = new Hex(hexes[i, j - 1].Points[(int)FlatVertice.MiddleRight], side, orientation, new Vector2Int(i, j));
                                }
                            }
                            break;
                        case Orientation.Pointy:
                            if (inLeftColumn) {
                                // Calculate from hex above and need to stagger the weakRows
                                if (i % 2 == 0) {
                                    hexes[i, j] = new Hex(hexes[i - 1, j].Points[(int)PointyVertice.BottomLeft], side, orientation, new Vector2Int(i, j));
                                } else {
                                    hexes[i, j] = new Hex(hexes[i - 1, j].Points[(int)PointyVertice.BottomRight], side, orientation, new Vector2Int(i, j));
                                }

                            } else {
                                // Calculate from Hex to the left
                                float x = hexes[i, j - 1].Points[(int)PointyVertice.UpperRight].x;
                                float y = hexes[i, j - 1].Points[(int)PointyVertice.UpperRight].y;
                                x += r;
                                y -= h;
                                hexes[i, j] = new Hex(x, y, side, orientation, new Vector2Int(i, j));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    } //end draw



    public void Draw() {
        // seems to be needed to avoid bottom and right from being chopped off
        width += 1;
        height += 1;


        //Prepare texture for drawing
        Material material = target.GetComponent<Renderer>().material;
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGB24, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        material.SetTexture(0, texture);

        //
        // Draw Hex Grid
        //
        for (int i = 0; i < hexes.GetLength(0); i++) {
            for (int j = 0; j < hexes.GetLength(1); j++) {
                texture.DrawPolygon(hexes[i, j].Points, gridColor);
            }
        }
        //
        // Draw Active Hex, if present
        //
        /*if (board.BoardState.ActiveHex != null) {
            //p.Color = board.BoardState.ActiveHexBorderColor;
            //p.Width = board.BoardState.ActiveHexBorderWidth;
            texture.DrawPolygon(board.BoardState.ActiveHex.Points, board.BoardState.ActiveHexBorderColor);
        }*/

        texture.Apply();
        --width;
        --height;
    }



    #region click related
    public bool PointInBoardRectangle(Vector2 point) {
        return PointInBoardRectangle(point.x, point.y);
    }

    public bool PointInBoardRectangle(float x, float y) {
        //
        // Quick check to see if X,Y coordinate is even in the bounding rectangle of the board.
        // Can produce a false positive because of the staggerring effect of hexes around the edge
        // of the board, but can be used to rule out an m_x,m_y point.
        //
        int topLeftX = (int)xOffset;
        int topLeftY = (int)yOffset;
        float bottomRightX = topLeftX + pixelWidth;
        float bottomRightY = topLeftY + pixelHeight;


        if (x > topLeftX && x < bottomRightX && y > topLeftY && y < bottomRightY) {
            return true;
        } else {
            return false;
        }

    }

    public Hex FindHexMouseClick(Vector2 point) {
        return FindHexMouseClick(point.x, point.y);
    }

    public Hex FindHexMouseClick(float x, float y) {
        Hex target = null;

        if (PointInBoardRectangle(x, y)) {
            for (int i = 0; i < hexes.GetLength(0); i++) {
                for (int j = 0; j < hexes.GetLength(1); j++) {
                    if (HexMath.InsidePolygon(hexes[i, j].Points, 6, new Vector2(x, y))) {
                        target = hexes[i, j];
                        break;
                    }
                }

                if (target != null) {
                    break;
                }
            }

        }

        return target;

    }
    #endregion

}