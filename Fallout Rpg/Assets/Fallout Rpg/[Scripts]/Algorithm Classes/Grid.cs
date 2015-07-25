using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
	public LayerMask _unwalkable_mask;
	public Vector2 _grid_world_size;
	public float _node_radius;
	public Node[,] _grid;

	float _node_diameter;
	int _grid_size_x, _grid_size_y;

	// Use this for initialization
	void Start () {
		//_alg = new Dijkstra(this);
		_node_diameter = _node_radius * 2;
		_grid_size_x = Mathf.RoundToInt(_grid_world_size.x / _node_diameter);
		_grid_size_y = Mathf.RoundToInt(_grid_world_size.y / _node_diameter);
		create_grid();
	}
	
	// Update is called once per frame
	void Update () {
		//_path = _alg.dijkstra(seeker.position, target.position);

	}
	public List<Node> _path;
	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(_grid_world_size.x,1,_grid_world_size.y));
		if (_grid != null){
			foreach(Node n in _grid) {
				Gizmos.color = (n._walkable) ? Color.green : Color.red;
				if(_path != null){
					if (_path.Contains(n)){
						Gizmos.color = Color.black;
					}
				}
				Gizmos.DrawCube(n._world_position, Vector3.one * (_node_diameter-.1f))	;
			}
		}
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();
		
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;
				
				int checkX = node._grid_x + x;
				int checkY = node._grid_y + y;
				
				if (checkX >= 0 && checkX < _grid_size_x && checkY >= 0 && checkY < _grid_size_y) {
					neighbours.Add(_grid[checkX,checkY]);
				}
			}
		}
		
		return neighbours;
	}

	public Node node_from_world_point(Vector3 world_position) {
		float percentX = (world_position.x + _grid_world_size.x/2) / _grid_world_size.x;
		float percentY = (world_position.z + _grid_world_size.y/2) / _grid_world_size.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		int x = Mathf.RoundToInt((_grid_size_x-1) * percentX);
		int y = Mathf.RoundToInt((_grid_size_y-1) * percentY);
		Debug.Log (x +" x");
		Debug.Log (y +" y");
		return _grid[x,y];
	}

	void create_grid(){
		_grid = new Node[_grid_size_x,_grid_size_y];
		Vector3 world_bottom_left = transform.position - Vector3.right * _grid_world_size.x/2 - Vector3.forward * _grid_world_size.y/2;
		for(int i = 0; i < _grid_size_x; i++){
			for(int j = 0; j < _grid_size_y; j++){
				Vector3 world_point = world_bottom_left + Vector3.right * (i * _node_diameter + _node_radius) + Vector3.forward * (j * _node_diameter + _node_radius);
				bool walkable = !(Physics.CheckSphere(world_point,_node_radius,_unwalkable_mask));
				_grid[i,j] = new Node(walkable,world_point,i,j);
			}	
		}
	}
}
