using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {
	private Vector3 _pos;
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

		//Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		locate_position();
	}

	void OnGUI(){
		GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, cursorWidth, cursorHeight), cursorImage); 
	}

	void locate_position() {

		//GetComponent<Animation>().Play("idle");
		if(Input.GetMouseButton(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit,1000)){
				_pos = new Vector3(hit.point.x,hit.point.y,hit.point.z);
				directionVector = hit.point - transform.position;
				directionVector.y = 0;
			}

		}
		//Cursor.SetCursor(cursorImage,Vector2.zero,CursorMode.Auto);
		move_to_position(hit);

	}
	void play_animation(string animation) {
			_animation = animation;
			GetComponent<Animation>().Play(_animation);
	}

	void move_to_position(RaycastHit hit) {
		// calculate the current target direction
		Vector3 destDir = hit.point - transform.position;
		destinationDistance = destDir.magnitude; // get the horizontal distance
		// object doesn't anything if below stopDistance:
		if (destinationDistance >= stopDistance){ // if farther than stopDistance...
			play_animation("run");
			targetRotation = Quaternion.LookRotation(destDir); // update target rotation...
			// turn gradually to target direction each frame:
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
			// move in its local forward direction (Translate default):
			transform.Translate(Vector3.forward * 4.5f * Time.deltaTime);  // move in forward direction 
		}//end if
		else
			play_animation("idle");

	}

}
