using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	public GameObject _player_character;
	public Camera _main_camera;
	public GameObject _pc;
	public GameObject go;
	public float _zoom;
	// Use this for initialization
	void Start () {
		_zoom = 5;
		_pc = GameObject.Find("PlayerCharacter");
		go = GameObject.Find("Player Spawn Point");
		_pc.transform.position = go.transform.position;
		//_pc = Instantiate(_player_character,go.transform.position,Quaternion.identity) as GameObject;
		_main_camera.transform.position = new Vector3(_pc.transform.position.x , _pc.transform.position.y  +5, _pc.transform.position.z -5);
		_main_camera.transform.Rotate(46,0,0);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
			_zoom++;
		else if(Input.GetAxis("Mouse ScrollWheel") > 0)
			_zoom--;
		_main_camera.transform.position = new Vector3(_pc.transform.position.x , _pc.transform.position.y  +_zoom, _pc.transform.position.z -_zoom );
	}
}
