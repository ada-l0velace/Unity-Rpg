using UnityEngine;
using System.Collections;

public class Node {

	public bool _walkable;
	public Vector3 _world_position;

	public Node(bool walkable, Vector3 world_position) {
		_walkable = walkable;
		_world_position = world_position;
	}
}
