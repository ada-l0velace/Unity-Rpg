using System;
using System.Collections.Generic;
using UnityEngine;

public class Hex : AStarNode {
    private Vector2[] m_points;
    private float m_side;
    private float m_h;
    private float m_r;
    private CellShape m_orientation;
    private float m_x;
    private float m_y;

    /// <param name="m_side">length of one m_side of the hexagon</param>
    public Hex(int x, int y, int side, CellShape orientation, Vector2Int pos) : base(pos, 6) {
        Initialize((float)x, (float)y, (float)side, orientation);
    }

    public Hex(float x, float y, float side, CellShape orientation, Vector2Int pos) : base(pos, 6) {
        Initialize(x, y, side, orientation);
    }

    public Hex(Vector2 point, float side, CellShape orientation, Vector2Int pos) : base(pos, 6) {
        Initialize(point.x, point.y, side, orientation);
    }

    /// <summary>
    /// Sets internal fields and calls CalculateVertices()
    /// </summary>
    private void Initialize(float x, float y, float side, CellShape orientation) {
        this.m_x = x;
        this.m_y = y;
        this.m_side = side;
        this.m_orientation = orientation;
        CalculateVertices();
    }

    /// <summary>
    /// Calculates the vertices of the hex based on m_orientation. Assumes that m_points[0] contains a value.
    /// </summary>
    private void CalculateVertices() {
        //  
        //  m_h = short length (outside)
        //  m_r = long length (outside)
        //  m_side = length of a m_side of the hexagon, all 6 are equal length
        //
        //  m_h = sin (30 degrees) m_x m_side
        //  m_r = cos (30 degrees) m_x m_side
        //
        //		 m_h
        //	     ---
        //   ----     |m_r
        //  /    \    |          
        // /      \   |
        // \      /
        //  \____/
        //
        // Flat m_orientation (scale is off)
        //
        //     /\
        //    /  \
        //   /    \
        //   |    |
        //   |    |
        //   |    |
        //   \    /
        //    \  /
        //     \/
        // Pointy m_orientation (scale is off)

        m_h = HexMath.CalculateH(m_side);
        m_r = HexMath.CalculateR(m_side);

        switch (m_orientation) {
            case CellShape.Flat:
                // m_x,m_y coordinates are top left point
                m_points = new Vector2[6];
                m_points[0] = new Vector2(m_x, m_y);
                m_points[1] = new Vector2(m_x + m_side, m_y);
                m_points[2] = new Vector2(m_x + m_side + m_h, m_y + m_r);
                m_points[3] = new Vector2(m_x + m_side, m_y + m_r + m_r);
                m_points[4] = new Vector2(m_x, m_y + m_r + m_r);
                m_points[5] = new Vector2(m_x - m_h, m_y + m_r);
                break;
            case CellShape.Pointy:
                //m_x,m_y coordinates are top center point
                m_points = new Vector2[6];
                m_points[0] = new Vector2(m_x, m_y);
                m_points[1] = new Vector2(m_x + m_r, m_y + m_h);
                m_points[2] = new Vector2(m_x + m_r, m_y + m_side + m_h);
                m_points[3] = new Vector2(m_x, m_y + m_side + m_h + m_h);
                m_points[4] = new Vector2(m_x - m_r, m_y + m_side + m_h);
                m_points[5] = new Vector2(m_x - m_r, m_y + m_h);
                break;
            default:
                throw new Exception("No CellShape defined for Hex object.");

        }

    }

    public CellShape Orientation {
        get {
            return m_orientation;
        }
        set {
            m_orientation = value;
        }
    }

    public Vector2[] Points {
        get { return m_points; }
        set { }
    }

    public float Side {
        get { return m_side; }
        set { }
    }

    public float H {
        get { return m_h; }
        set { }
    }

    public float R {
        get { return m_r; }
        set { }
    }

    
}