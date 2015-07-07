using UnityEngine;
using System.Collections;
using System;

public class Character : Entity {
	private string _name;
	private int _level;
	private uint _xp;
	private PrimaryStat [] _primary_stats;
	private DerivedStat [] _derived_stats;
	// derived_stats
	// skills

	public void Awake(){
		_name = "";
		_level = 0;
		_xp = 0;
		_primary_stats = new PrimaryStat[Enum.GetValues (typeof(StatName)).Length];
		_derived_stats = new DerivedStat[Enum.GetValues (typeof(DerivedName)).Length];
		setup_primary_stats ();
		setup_derived_stats ();
		setup_modifiers ();
	}

	public void start(){
	
	}

	public void update(){
	
	}

	public String name {
		get	{ return _name;}
		set	{ _name = value;}
	}

	public int level {
		get	{ return _level;}
		set	{ _level = value;}
	}

	public uint xp {
		get	{ return _xp;}
		set	{ _xp = value;}
	}

	public void add_exp(uint experience) {
		_xp +=  experience;

	}

	public void calculate_level() {
	
	}

	public void setup_primary_stats(){
		for ( int i = 0; i < _primary_stats.Length;i++) {
			_primary_stats [i] = new PrimaryStat();
		}
	}

	public void setup_derived_stats(){
		for ( int i = 0; i < _derived_stats.Length;i++) {
			_derived_stats [i] = new DerivedStat();
		}
	}

	public PrimaryStat get_primary_stats(int index){
		return _primary_stats [index];
	}

	public DerivedStat get_derived_stats(int index){
		return _derived_stats [index];
	}

	private void setup_modifiers(){
		//Health
		ModifyingStat health_m = new ModifyingStat ();
		health_m.primary_stat = get_primary_stats ((int)StatName.Endurance);
		health_m.ratio = .5f;
		get_derived_stats ((int)DerivedName.Health).add_modifier (health_m);
		//ActionPoints
		ModifyingStat action_points = new ModifyingStat ();
		action_points.primary_stat = get_primary_stats ((int)StatName.Agility);
		action_points.ratio = .5f;
		get_derived_stats ((int)DerivedName.ActionPoints).add_modifier (action_points);
		//Health
		ModifyingStat damage_m = new ModifyingStat ();
		damage_m.primary_stat = get_primary_stats ((int)StatName.Strength);
		damage_m.ratio = .5f;
		get_derived_stats ((int)DerivedName.Damage).add_modifier (damage_m);
		//Health
		ModifyingStat armor_m = new ModifyingStat ();
		armor_m.primary_stat = get_primary_stats ((int)StatName.Endurance);
		armor_m.ratio = .5f;
		get_derived_stats ((int)DerivedName.Armor).add_modifier (armor_m);

	}

	public void update_stats() {
		for ( int i = 0; i < _derived_stats.Length;i++) {
			_derived_stats [i].update();
		}
	}
}
