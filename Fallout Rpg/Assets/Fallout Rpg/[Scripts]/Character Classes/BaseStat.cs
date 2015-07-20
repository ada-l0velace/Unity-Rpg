using UnityEngine;
using System.Collections;

/// <summary>
/// Base stat.cs
/// 
/// This is the base class for a stats in game
/// </summary>
public class BaseStat {
	private int _base_value;
	private int _exp_to_level;
	private float _level_modifier;
	private int _buff_value;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseStat"/> class.
	/// </summary>
	public BaseStat() {
		_base_value = 0;
		_buff_value = 0;
		_level_modifier = 1.0f;
		_exp_to_level = 100;
	}

	/// <summary>
	/// Calculates the xp to level.
	/// </summary>
	public int calculate_exp_to_level() {
		return (int)(_exp_to_level * _level_modifier);
	}

	/// <summary>
	/// Assign the new value to _exp_to_level and then increases the _basevalue by one.
	/// </summary>
	public void level_up() {
		_exp_to_level = calculate_exp_to_level ();
		base_value++;
	}

	/// <summary>
	/// Recalculate the adjusted base value and return it...
	/// </summary>
	/// <value>The adjusted_base_value.</value>
	public int adjusted_base_value {
		get{return _base_value + _buff_value;}
	}

	#region Setters and Getters
	/// <summary>
	/// Gets or sets the base_value.
	/// </summary>
	/// <value>The _base_value.</value>
	public int base_value{
		get{return _base_value;}
		set{ _base_value = value;}
	}

	/// <summary>
	/// Gets or sets the exp_to_level.
	/// </summary>
	/// <value>The _exp_to_level.</value>
	public int exp_to_level{
		get{return _exp_to_level;}
		set{ _exp_to_level = value;}
	}

	/// <summary>
	/// Gets or sets the level_modifier.
	/// </summary>
	/// <value>The _level_modifier.</value>
	public float level_modifier{
		get{ return _level_modifier;}
		set{ _level_modifier = value;}
	}

	/// <summary>
	/// Gets or sets the buff_value.
	/// </summary>
	/// <value>The _buff_value.</value>
	public int buff_value{
		get{return _buff_value;}
		set{ _buff_value = value;}
	}
	#endregion

	/// <summary>
	/// Minimum the specified value.
	/// </summary>
	/// <param name="value">Value.</param>
	protected int min (int value) {
		return (value > 0) ? value : 1;
	}
}
