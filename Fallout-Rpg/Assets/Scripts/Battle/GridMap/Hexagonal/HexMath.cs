using System;
using UnityEngine;

	class HexMath
	{

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

		public static double DegreesToRadians(double degrees)
		{
			//http://en.wikipedia.org/wiki/Radians
			return degrees * System.Math.PI / 180;
		}

		public static double RadiansToDegrees(double radians)
		{
			return radians * 180 / System.Math.PI;
		}

		public static float ConvertToFloat(double d)
		{
			return (float)d;
		}

		public static float ConvertToFloat(int i)
		{
			return (float)i;
		}

		/// <summary>
		/// Outside triangle m_side (short)
		/// </summary>
		public static float CalculateH(float side)
		{
			return ConvertToFloat(System.Math.Sin(DegreesToRadians(30)) * side);
		}

		/// <summary>
		/// Outside triangle m_side (long)
		/// </summary>
		public static float CalculateR(float side)
		{
			return ConvertToFloat(System.Math.Cos(DegreesToRadians(30)) * side);
		}


		public static bool InsidePolygon(Vector2[] polygon, int N, Vector2 p)
		{
			//http://astronomy.swin.edu.au/~pbourke/geometry/insidepoly/
			//
			// Slick algorithm that checks if a point is inside a polygon.  Checks how may time a weakLine
			// origination from point will cross each m_side.  An odd result means inside polygon.
			//
			int counter = 0;
			int i;
			double xinters;
			Vector2 p1,p2;
			
			p1 = polygon[0];
			for (i=1;i<=N;i++) 
			{
				p2 = polygon[i % N];
				if (p.y > System.Math.Min(p1.y,p2.y)) 
				{
					if (p.y <= System.Math.Max(p1.y,p2.y)) 
					{
						if (p.x <= System.Math.Max(p1.x,p2.x)) 
						{
							if (p1.y != p2.y)
							{
								xinters = (p.y-p1.y)*(p2.x-p1.x)/(p2.y-p1.y)+p1.x;
								if (p1.x == p2.y || p.x <= xinters)
									counter++;
							}
						}
					}
				}	
				p1 = p2;
			}

			if (counter % 2 == 0)
				return false;
			else
				return true;
		}

		
	}