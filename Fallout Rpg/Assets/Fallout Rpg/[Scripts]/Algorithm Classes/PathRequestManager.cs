using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class PathRequestManager : MonoBehaviour {

	Queue<PathRequest> path_request_queue = new Queue<PathRequest>();
	PathRequest current_path_request;

	static PathRequestManager instance;
	PathFinding path_finding;

	bool is_processing_path;

	void Awake() {
		//Debug.Log("I'm awake");
		instance = this;
		path_finding = GetComponent<PathFinding>();
	}

	public static void RequestPath(Vector3 path_start, Vector3 path_end, Action<Vector3[], bool> callback) {
		PathRequest new_request = new PathRequest(path_start, path_end, callback);
		instance.path_request_queue.Enqueue(new_request);

		instance.TryProcessNext();
	}

	void TryProcessNext() {
		if (! is_processing_path && path_request_queue.Count > 0) {
			current_path_request = path_request_queue.Dequeue();
			is_processing_path = true;
			path_finding.StartFindPath(current_path_request.path_start, current_path_request.path_end);
		}
	}

	public void FinishingProcessingPath ( Vector3[] path, bool sucess) {
		current_path_request.callback(path, sucess);
		is_processing_path = false;
		TryProcessNext();
	}

	struct PathRequest {
		public Vector3 path_start;
		public Vector3 path_end;
		public Action<Vector3[], bool> callback;

		public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
			path_start = _start;
			path_end = _end;
			callback = _callback;
		}
	} 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
