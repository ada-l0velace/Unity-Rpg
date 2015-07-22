using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	public GameObject _player_character;
	public Camera _main_camera;
	public GameObject _pc;
	public GameObject go;
	public float z_offset = -9.0f;
	public float y_offset = 196.0f;
	public float x_offset = 203.0f;
	public float x_rot_offset = 29.0f;
	public float y_rot_offset = -90.0f;
	// Use this for initialization
	void Start () {
		_pc = GameObject.Find("Player Character");
		go = GameObject.Find("Player Spawn Point");
		_pc.transform.position = go.transform.position;
		//_pc = Instantiate(_player_character,go.transform.position,Quaternion.identity) as GameObject;
		_main_camera.transform.position = new Vector3(_pc.transform.position.x , _pc.transform.position.y  +30, _pc.transform.position.z -30);
		_main_camera.transform.Rotate(46,0,0);
		_pc.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
	}
	// Update is called once per frame
	void Update () {
		_main_camera.transform.position = new Vector3(_pc.transform.position.x , _pc.transform.position.y  +30, _pc.transform.position.z -30);
	}
}
