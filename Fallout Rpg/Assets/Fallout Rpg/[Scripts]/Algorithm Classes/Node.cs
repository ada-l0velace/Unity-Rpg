using UnityEngine;
using System.Collections;

public class Node {

	public bool _walkable;
	public Vector3 _world_position;
	public int _grid_x;
	public int _grid_y;
	public int gCost;
	public int hCost;
	public Node parent;

	public Node(bool walkable, Vector3 world_position, int grid_x, int grid_y) {
		_walkable = walkable;
		_world_position = world_position;
		_grid_x = grid_x;
		_grid_y = grid_y;
	}

	public int fCost  {
		get {
			return gCost + hCost;
		}
	}
}
