using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class CharacterGenerator : Entity {
	private Player _player;
	private string _adding_xp = "0";
	// Use this for initialization
	void Start () {
		_player = new Player ();
		_player.Awake();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		GUI.Label (new Rect (10, 10, 50, 25), "Name: ");
		_player.name = GUI.TextArea (new Rect (65, 10, 100, 25),_player.name);
		GUI.Label (new Rect (180, 10, 500, 25), "Level: " + _player.level.ToString() + "(" + _player.xp.ToString() + "/" + _player.xp_to_level.ToString()+ ")");
		_adding_xp = GUI.TextArea (new Rect (380, 10, 100, 25),_adding_xp);
		_adding_xp = Regex.Replace(_adding_xp, @"[ ^a-zA-Z ]", "");
		if(GUI.Button(new Rect(490, 10, 100, 25), "add xp")) {
			_player.add_exp(Convert.ToUInt32(_adding_xp));
		}
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
