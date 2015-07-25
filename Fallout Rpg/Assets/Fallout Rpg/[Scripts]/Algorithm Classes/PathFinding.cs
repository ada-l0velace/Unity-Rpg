using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


public class PathFinding : MonoBehaviour {
	PathRequestManager request_manager;
	Grid grid;
	
	void Awake() {
		request_manager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}
	void Start() {

		//FindPath(seeker.position,target.position);
	}
	void Update() {
		//if( Input.GetButtonDown("Jump"))
			//FindPath(seeker.position,target.position);
	}
	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		StartCoroutine(FindPath(startPos,targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
		Stopwatch sw = new Stopwatch();
		sw.Start();

		Vector3[] waypoints = new Vector3 [0];
		bool path_sucess = false;

		Node startNode = grid.node_from_world_point(startPos);
		Node targetNode = grid.node_from_world_point(targetPos);

		if( startNode._walkable && targetNode._walkable ) {
			List<Node> openSet = new List<Node>();
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0) {
				Node currentNode = openSet[0];
				for (int i = 1; i < openSet.Count; i ++) {
					if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
						currentNode = openSet[i];
					}
				}
				
				openSet.Remove(currentNode);
				closedSet.Add(currentNode);
				
				if (currentNode == targetNode) {
					sw.Stop();
					print ("Path found: " + sw.ElapsedMilliseconds + " ms");
					//RetracePath(startNode,targetNode);
					//return;
					path_sucess = true;
					break;
				}
				
				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
					if (!neighbour._walkable || closedSet.Contains(neighbour)) {
						continue;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
		}
		yield return null;
		if (path_sucess) {
			waypoints = RetracePath(startNode,targetNode);
		}
		request_manager.FinishingProcessingPath(waypoints, path_sucess);
	}
	
	Vector3 [] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			//path.Add(currentNode);
			path.Insert(0,currentNode);
			currentNode = currentNode.parent;
		}

		//path.Reverse();
		//grid._path = path;
		Vector3[] waypoints = SimplifyPath(path);
		return waypoints;
	}

	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i ++) {
			//Vector2 directionNew = new Vector2(path[i-1]._grid_x - path[i]._grid_x, path[i-1]._grid_y - path[i]._grid_y);
			//if (directionNew != directionOld) {
			waypoints.Add(path[i]._world_position);
			//}
			//directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA._grid_x - nodeB._grid_x);
		int dstY = Mathf.Abs(nodeA._grid_y - nodeB._grid_y);
		
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}
