using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {
	public float _speed;
	private Vector3 dest_pos;
	private Vector3 _pos;
	public CharacterController controler;
	private Vector3 targetPosition;
	private Vector3 directionVector;
	public float smooth;
	public GameObject player;
	public float stopDistance = 0.1f;
	private float destinationDistance;
	private Quaternion targetRotation;
	public float rotateSpeed = 90f;
	public Vector3 myold;
	//CharacterMotor motor;
	// Use this for initialization
	void Start () {
		//targetPosition = GameObject.Find("Player Spawn Point").transform.position;
		transform.Rotate(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)) {
			locate_position();
		}
	}

	void locate_position() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit,1000)){
			myold = transform.position;
			_pos = new Vector3(hit.point.x,hit.point.y,hit.point.z);
			dest_pos = hit.point;
			directionVector = hit.point - transform.position;
			directionVector.y = 0;
			// 
		}
		move_to_position(hit);

	}

	void move_to_position(RaycastHit hit) {
		// calculate the current target direction
		Vector3 destDir = hit.point - transform.position;
		destinationDistance = destDir.magnitude; // get the horizontal distance
		// object doesn't anything if below stopDistance:
		if (destinationDistance >= stopDistance){ // if farther than stopDistance...
			targetRotation = Quaternion.LookRotation(destDir); // update target rotation...
			// turn gradually to target direction each frame:
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
			// move in its local forward direction (Translate default):
			transform.Translate(Vector3.forward * 4.5f * Time.deltaTime);  // move in forward direction 
		}//end if
		
	}
}
