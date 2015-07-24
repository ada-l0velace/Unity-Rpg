using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public LayerMask _unwalkable_mask;
	public Vector2 _grid_world_size;
	public float _node_radius;
	Node[,] _grid;
	float _node_diameter;
	int _grid_size_x, _grid_size_y;

	// Use this for initialization
	void Start () {
		_node_diameter = _node_radius * 2;
		_grid_size_x = Mathf.RoundToInt(_grid_world_size.x / _node_diameter);
		_grid_size_y = Mathf.RoundToInt(_grid_world_size.y / _node_diameter);
		create_grid();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos(){

		Gizmos.DrawWireCube(transform.position, new Vector3(_grid_world_size.x,1,_grid_world_size.y));
		if (_grid != null){
			foreach(Node n in _grid) {
				Gizmos.color = (n._walkable) ? Color.green : Color.red;
				Gizmos.DrawCube(n._world_position, Vector3.one * (_node_diameter-0.1f));
			}
		}
	}

	void create_grid(){
		_grid = new Node[_grid_size_x,_grid_size_y];
		Vector3 world_bottom_left = transform.position - Vector3.right * _grid_world_size.x/2 - Vector3.forward * _grid_world_size.y/2;
		for(int i = 0; i < _grid_size_x; i++){
			for(int j = 0; j < _grid_size_y; j++){
				Vector3 world_point = world_bottom_left + Vector3.right * (i * _node_diameter + _node_radius) + Vector3.forward * (j * _node_diameter + _node_radius);
				bool walkable = !(Physics.CheckSphere(world_point,_node_radius,_unwalkable_mask));
				_grid[i,j] = new Node(walkable,world_point);
			}	
		}
	}
}
