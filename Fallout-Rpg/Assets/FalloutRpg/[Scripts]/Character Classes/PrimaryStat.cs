using UnityEngine;
using System.Collections;

public class PrimaryStat : BaseStat {
	public PrimaryStat(){
		exp_to_level = 50;
		level_modifier = 1.0f;
	}
	public PrimaryStat(int basev){
		base_value = basev;
		exp_to_level = 50;
		level_modifier = 1.0f;
	}
}
public enum StatName {
	Strength,
	Agility,
	Luck,
	Charisma,
	Perception,
	Intelligence,
	Endurance
}