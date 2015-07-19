using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModifiedStat : BaseStat {
	private List<ModifyingStat> _mods; // list of stats that modify a derived stat
	private List<ModifyingStat> _xp_mods; // list of stats that modify a derived stat
	private int _mod_value; // the amount added to the base value from the modifiers
	private int _mod_xp_value; // the amount added to the base value from the experience modifiers

	public ModifiedStat(){
		_mods = new List<ModifyingStat> ();
		_xp_mods = new List<ModifyingStat> ();
		_mod_value = 0;
		_mod_xp_value = 0;
	}

	public void add_modifier(ModifyingStat mod){
		_mods.Add(mod);
	}

	public void add_xp_modifier(ModifyingStat mod){
		_xp_mods.Add(mod);
	}

	public void calculate_mod_xp_value(){
		if (_xp_mods.Count > 0)
			foreach (ModifyingStat stat in _xp_mods)
				_mod_xp_value += (int)(stat.primary_stat.adjusted_base_value * stat.ratio);
	}

	public void calculate_mod_value(){
		_mod_value = 0;

		if (_mods.Count > 0)
			foreach (ModifyingStat stat in _mods)
				_mod_value += (int)(stat.primary_stat.adjusted_base_value * stat.ratio);
	}

	public new int adjusted_base_value {
		get{return min(base_value + buff_value + _mod_value + _mod_xp_value);}
	}

	public new void level_up() {
		calculate_mod_xp_value();
	}

	public void update(){
		calculate_mod_value();
	}
}


public struct ModifyingStat {
	public PrimaryStat primary_stat;
	public float ratio;
}