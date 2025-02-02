﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dijkstra  {

	public Dictionary <Node,float> dist ;
	public Dictionary <Node,Node> prev ;
	List<Node> Q = new List<Node>();
	Grid _grid;

	public Dijkstra(Grid grid) {
		_grid = grid;
		dist = new Dictionary<Node,float>();
		prev = new Dictionary<Node,Node>();
	}

	// Update is called once per frames
	void Update () {

	}
	// ] 
	public Stack<Node> dijkstra(Vector3 startPos, Vector3 targetPos) {
		Node startNode = _grid.node_from_world_point(startPos);
		Node targetNode = _grid.node_from_world_point(targetPos);
		dist[startNode] = .0f;
		foreach(Node n in _grid._grid) {	
			if (n != startNode) {
				dist[n] = Mathf.Infinity;
				prev[n] = null;
			}
			Q.Add(n); 
		}
		//Debug.Log("got here");
		while (Q.Count > 0){
			float min_dist = Mathf.Infinity;
			Node min_dist_node = null;
			foreach(Node n in Q) {
				if (dist[n] < min_dist) {
					min_dist = dist[n];
					min_dist_node = n;
				}
			}
			Q.Remove(min_dist_node);
			if (min_dist_node == targetNode) {
				Stack<Node> S = new Stack<Node>();
				while( prev[min_dist_node] != null){
					S.Push(min_dist_node);
					min_dist_node = prev [min_dist_node];
				}
				return S;
			}
			if (dist[min_dist_node] == Mathf.Infinity)
				break;

			foreach(Node neighbour in _grid.GetNeighbours(min_dist_node)) {
				float alt = dist [min_dist_node] + Vector3.Distance(min_dist_node._world_position, neighbour._world_position);
				if (alt < dist [neighbour]) {
					dist [neighbour] = alt;
					prev [neighbour] = min_dist_node;
				}
			}
		}
		return null;
	}

}
