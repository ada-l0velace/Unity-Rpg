using UnityEngine;
using System.Collections;

public class MoveSeeker : MonoBehaviour {
	string _key_up;
	string _key_down;
	string _key_left;
	string _key_right;
	float _speed = 4.1f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKey(KeyCode.UpArrow)){
			transform.Translate(Vector3.forward * _speed * Time.deltaTime);
		}
		if ( Input.GetKey(KeyCode.DownArrow)){
			transform.Translate(Vector3.back * _speed * Time.deltaTime);
		}
		if ( Input.GetKey(KeyCode.LeftArrow)){
			transform.Translate(Vector3.left * _speed * Time.deltaTime);
		}
		if ( Input.GetKey(KeyCode.RightArrow)){
			transform.Translate(Vector3.right * _speed * Time.deltaTime);
		}
	}

}
