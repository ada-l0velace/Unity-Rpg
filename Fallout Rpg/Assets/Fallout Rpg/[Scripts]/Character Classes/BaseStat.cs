using UnityEngine;
using System.Collections;

public class BaseStat {
	private int _base_value;
	private int _exp_to_level;
	private float _level_modifier;
	private int _buff_value;

	public BaseStat() {
		_base_value = 0;
		_buff_value = 0;
		_level_modifier = 1.0f;
		_exp_to_level = 100;
	}

	public int calculate_exp_to_level() {
		return (int)(_exp_to_level * _level_modifier);
	}

	public void level_up() {
		_exp_to_level = calculate_exp_to_level ();
		base_value++;
	}

#region Setters and Getters
	public int base_value{
		get{return _base_value;}
		set{ _base_value = value;}
	}
	public int exp_to_level{
		get{return _exp_to_level;}
		set{ _exp_to_level = value;}
	}
	public float level_modifier{
		get{ return _level_modifier;}
		set{ _level_modifier = value;}
	}
	public int buff_value{
		get{return _buff_value;}
		set{ _buff_value = value;}
	}
#endregion
}
