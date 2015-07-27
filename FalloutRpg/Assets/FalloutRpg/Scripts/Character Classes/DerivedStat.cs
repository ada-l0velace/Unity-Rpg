using UnityEngine;
using System.Collections;

public class DerivedStat : ModifiedStat {
	private int _cur_value;

	public DerivedStat (){
		_cur_value = 0;
		exp_to_level = 50;
		level_modifier = 1.1f;
	}
	public DerivedStat (int baseV){
		base_value = baseV;
		_cur_value = 0;
		exp_to_level = 50;
		level_modifier = 1.1f;
	}

	public int cur_value {
		get {return _cur_value;}
		set {_cur_value = value; }
	}
}

public enum DerivedName {
	CarryWeight,
	HitPoints,
	MeleeDamage,
	HealingRate,
	PoisonResistance,
	RadiationResistance,
	ActionPoints,
	Evasion,
	CriticalChance,
	Sequence
}