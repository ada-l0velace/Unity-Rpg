using UnityEngine;
using System.Collections;
using System;

public class Character : Entity {
	private string _name;
	private int _level;
	private uint _xp;
	// primary_stats
	// derived_stats
	// skills

	void Awake(){
		_name = "";
		_level = 0;
		_xp = 0;
	}
// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void setup_primary_stats(){
		
	}

	private void setup_derived_stats(){
		
	}

	private void get_primary_stats(int index){
		
	}

	private void get_derived_stats(int index){
		
	}
}
