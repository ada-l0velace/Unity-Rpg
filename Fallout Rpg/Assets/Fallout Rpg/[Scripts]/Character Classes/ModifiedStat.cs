using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModifiedStat : BaseStat {
	private List<ModifyingStat> _mods; // list of stats that modify a derived stat
	private int _mod_value; // the amount added to the base value from the modifiers

	public ModifiedStat(){
		_mods = new List<ModifyingStat> ();
		_mod_value = 0;
	}

	public add_modifier(ModifiedStat mod){
		_mods.Add(mod);
	}

	public void calculate_mod_value(){
		_mod_value = 0;

		if(_mods.Count > 0)
			foreach(ModifyingStat stat in _mods)
				_mod_value += (int)(stat.primary_stat.adjusted_base_value * stat.ratio)
	}
}


public struct ModifyingStat {
	public PrimaryStat primary_stat;
	public floar ratio;
}