using UnityEngine;
using System.Collections;


public class ClickToMove : MonoBehaviour {
	private Vector3 _pos;
	private Vector3 [] _path;
	int targetIndex;
	private Vector3 directionVector;
	public float stopDistance = 1.8f;
	private float destinationDistance;
	private Quaternion targetRotation;
	public float rotateSpeed = 10f;
	public RaycastHit hit;
	private string _animation;
	public Texture2D cursorImage;
	public Texture2D cursorImage_run;
	private int cursorWidth = 32;
	private int cursorHeight = 32;

	// Use this for initialization
	void Start () {
		hit.point = GameObject.Find("Player Spawn Point").transform.position;
	}

	public void OnPathFound(Vector3 [] new_path, bool path_succeful) {
		if ( path_succeful && new_path.Length > 0) {
			targetIndex = 0;
			_path = new_path;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	// Update is called once per frame
	void Update () {
		locate_position();
	}

	void OnGUI(){

	}

	void locate_position() {
		if(Input.GetMouseButton(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit,1000)){
				_pos = new Vector3(hit.point.x,hit.point.y,hit.point.z);
				PathRequestManager.RequestPath(transform.position,_pos, OnPathFound);
				directionVector = hit.point - transform.position;
				directionVector.y = 0;

			}

		}
		//Cursor.SetCursor(cursorImage,Vector2.zero,CursorMode.Auto);
	}

	IEnumerator FollowPath() {
		Vector3 currentWaypoint = _path[0];
		while (true) {
			// calculate the current target direction
			Vector3 destDir = currentWaypoint - transform.position;
			destinationDistance = destDir.magnitude; // get the horizontal distance

			if (destinationDistance <= stopDistance) {
				targetIndex ++;
				if (targetIndex >= _path.Length) {
					play_animation("idle");
					targetIndex = 0;
					_path = new Vector3[0];
					yield break;
				}
				currentWaypoint = _path[targetIndex];
			}
			play_animation("run");

			// object doesn't anything if below stopDistance:
			if (destinationDistance >= stopDistance){ // if farther than stopDistance...
				targetRotation = Quaternion.LookRotation(destDir); // update target rotation...
				// turn gradually to target direction each frame:
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
				// move in its local forward direction (Translate default):
				transform.Translate(Vector3.forward * 4.5f * Time.deltaTime);  // move in forward direction 
				//transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,4.5f * Time.deltaTime);
			}//end if
			//transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,4.5f * Time.deltaTime);
			yield return null;
			
		}
	}

	void play_animation(string animation) {
			_animation = animation;
			GetComponent<Animation>().Play(_animation);
	}

	public void OnDrawGizmos() {
		if (_path != null) {
			for (int i = targetIndex; i < _path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(_path[i], Vector3.one);
				
				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, _path[i]);
				}
				else {
					Gizmos.DrawLine(_path[i-1],_path[i]);
				}
			}
		}
	}

}
