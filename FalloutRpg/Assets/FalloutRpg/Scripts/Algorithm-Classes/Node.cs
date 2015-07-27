using UnityEngine;
using System.Collections;

public class Node : IHeapItem <Node> {

	public bool _walkable;
	public Vector3 _world_position;
	public int _grid_x;
	public int _grid_y;
	public int gCost;
	public int hCost;
	public int heapIndex;
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

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node node_to_compare) {
		int compare = fCost.CompareTo(node_to_compare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(node_to_compare.hCost);
		}
		return -compare;
	}
}
