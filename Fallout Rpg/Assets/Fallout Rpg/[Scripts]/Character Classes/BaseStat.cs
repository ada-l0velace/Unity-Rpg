using UnityEngine;
using System.Collections;

public class BaseStat {
	private int base_value;
	private int exp_to_level;
	private float level_modifier;
	private int buff_value;

	public BaseStat() {
		base_value = 0;
		buff_value = 0;
		level_modifier = 1.0f;
		exp_to_level = 100;
	}

	public int calculate_exp_to_level() {
		return (int)(exp_to_level * level_modifier);
	}

	public void level_up() {
		exp_to_level = calculate_exp_to_level ();
		base_value++;
	}
}
