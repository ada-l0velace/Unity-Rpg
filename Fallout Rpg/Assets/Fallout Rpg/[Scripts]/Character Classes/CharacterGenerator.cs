using UnityEngine;
using System.Collections;
using System;

public class CharacterGenerator : MonoBehaviour {
	private Player _player;	
	// Use this for initialization
	void Start () {
		_player = new Player ();
		_player.Awake();
		for(int i = 0; i < Enum.GetValues(typeof(StatName)).Length;i++){
			_player.get_primary_stats(i).base_value = 10;
		}
		_player.update_stats();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.Label (new Rect (10, 10, 50, 25), "Name: ");
		_player.name = GUI.TextArea (new Rect (65, 10, 100, 25),_player.name);
		for(int i = 0; i < Enum.GetValues(typeof(StatName)).Length;i++){
			GUI.Label(new Rect(10,40 + (i * 25),100,25), ((StatName)i).ToString());
			GUI.Label(new Rect(115,40 + (i * 25),30,25), (_player.get_primary_stats(i).adjusted_base_value.ToString()));
			if(GUI.Button(new Rect(150,40 + (i * 25),25,25), "+")) {
				_player.get_primary_stats(i).base_value++;
				_player.update_stats();
			}
			if(GUI.Button(new Rect(180,40 + (i * 25),25,25), "-")) {
				_player.get_primary_stats(i).base_value--;
				_player.update_stats();
			}
		}
		for(int i = 0; i < Enum.GetValues(typeof(DerivedName)).Length;i++){
			GUI.Label(new Rect(250,40 + (i * 25),100,25), ((DerivedName)i).ToString());
			GUI.Label(new Rect(375,40 + (i * 25),30,25), (_player.get_derived_stats(i).adjusted_base_value.ToString()));
		}
	}
}
