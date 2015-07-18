using UnityEngine;
using System.Collections;
using System;

public class Character : Entity {
	private string _name;
	private int _level;
	private uint _xp;
	private PrimaryStat [] _primary_stats;
	private DerivedStat [] _derived_stats;
	private int STARTING_PSTATS = 5;

	// Primary Stats 
	private const int STRENGTH = (int)StatName.Strength;
	private const  int PERCEPTION = (int)StatName.Perception;
	private const  int ENDURANCE = (int)StatName.Endurance;
	private const  int CHARISMA = (int)StatName.Charisma;
	private const  int INTELLIGENCE = (int)StatName.Intelligence;
	private const  int AGILITY = (int)StatName.Agility;
	private const  int LUCK = (int)StatName.Luck;

	// Derived Stats
	private const int CARRY_WEIGHT = (int)DerivedName.CarryWeight;
	private const  int HIT_POINTS = (int)DerivedName.HitPoints;
	private const  int MELEE_DAMAGE = (int)DerivedName.MeleeDamage;
	private const  int HEALING_RATE = (int)DerivedName.HealingRate;
	private const  int POISON_RESISTANCE = (int)DerivedName.PoisonResistance;
	private const  int RADIATION_RESISTANCE = (int)DerivedName.RadiationResistance;
	private const  int ACTION_POINTS = (int)DerivedName.ActionPoints;
	private const  int EVASION = (int)DerivedName.Evasion;
	private const  int CRITICAL_CHANCE = (int)DerivedName.CriticalChance;
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
		update_stats ();
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
			_primary_stats [i] = new PrimaryStat(STARTING_PSTATS);
		}
	}

	public void setup_derived_stats(){
		for ( int i = 0; i < _derived_stats.Length;i++) {
			switch(i)
			{
				case CARRY_WEIGHT:
					_derived_stats [i] = new DerivedStat(25);
					break;
				case HIT_POINTS:
					_derived_stats [i] = new DerivedStat(15);
					break;
				case MELEE_DAMAGE:
					_derived_stats [i] = new DerivedStat(-5);
					break;
				case HEALING_RATE:
					_derived_stats [i] = new DerivedStat();
					break;
				case POISON_RESISTANCE:
					_derived_stats [i] = new DerivedStat();
					break;
				case RADIATION_RESISTANCE:
					_derived_stats [i] = new DerivedStat();
					break;
				case ACTION_POINTS:
					_derived_stats [i] = new DerivedStat(5);
					break;
				case EVASION:
					_derived_stats [i] = new DerivedStat();
					break;
				case CRITICAL_CHANCE:
					_derived_stats [i] = new DerivedStat();
					break;
			}
		}
	}

	public PrimaryStat get_primary_stats(int index){
		return _primary_stats [index];
	}

	public DerivedStat get_derived_stats(int index){
		return _derived_stats [index];
	}

	/// <summary>
	/// Method to define game stats
	/// </summary>
	/// <remarks>this method runs at Awake().</remarks>
	private void setup_modifiers(){

		//Carry Weight
		ModifyingStat carry_weight_strength = new ModifyingStat ();
		carry_weight_strength.primary_stat = get_primary_stats (STRENGTH);
		carry_weight_strength.ratio = 25.0f;
		get_derived_stats (CARRY_WEIGHT).add_modifier (carry_weight_strength);

		//Hit points
		ModifyingStat hit_points_endurance = new ModifyingStat (); //Endurance modifier for hit points
		hit_points_endurance.primary_stat = get_primary_stats (ENDURANCE);
		hit_points_endurance.ratio = 2.0f;
		ModifyingStat hit_points_strength = new ModifyingStat (); //Strength modifier for hit points
		hit_points_strength.primary_stat = get_primary_stats (STRENGTH);
		hit_points_strength.ratio = 1.0f;
		//adds the modifiers to the stats :D
		get_derived_stats (HIT_POINTS).add_modifier (hit_points_endurance);
		get_derived_stats (HIT_POINTS).add_modifier (hit_points_strength);

		//Action Points
		ModifyingStat action_points = new ModifyingStat ();
		action_points.primary_stat = get_primary_stats (AGILITY);
		action_points.ratio = .5f;
		get_derived_stats (ACTION_POINTS).add_modifier (action_points);

		//Melee Damage
		ModifyingStat melee_damage = new ModifyingStat ();
		melee_damage.primary_stat = get_primary_stats (STRENGTH);
		melee_damage.ratio = 1.0f;
		get_derived_stats (MELEE_DAMAGE).add_modifier (melee_damage);

		//Healing Rate
		ModifyingStat healing_rate_endurance = new ModifyingStat ();
		healing_rate_endurance.primary_stat = get_primary_stats (ENDURANCE);
		healing_rate_endurance.ratio = 1.0f / 3.0f;
		get_derived_stats (HEALING_RATE).add_modifier (healing_rate_endurance);

		//Poison Resistance
		ModifyingStat poison_res_endurance = new ModifyingStat ();
		poison_res_endurance.primary_stat = get_primary_stats (ENDURANCE);
		poison_res_endurance.ratio = 5.0f;
		get_derived_stats (POISON_RESISTANCE).add_modifier (poison_res_endurance);

		//Radiation Resistance
		ModifyingStat radiation_res_endurance = new ModifyingStat ();
		radiation_res_endurance.primary_stat = get_primary_stats (ENDURANCE);
		radiation_res_endurance.ratio = 2.0f;
		get_derived_stats (RADIATION_RESISTANCE).add_modifier (radiation_res_endurance);

		//Evasion, chance to get hitted
		ModifyingStat evasion = new ModifyingStat ();
		evasion.primary_stat = get_primary_stats (AGILITY);
		evasion.ratio = 1.0f;
		get_derived_stats (EVASION).add_modifier (evasion);

		//Critical Chance
		ModifyingStat critical_chance_luck = new ModifyingStat ();
		critical_chance_luck.primary_stat = get_primary_stats (LUCK);
		critical_chance_luck.ratio = 1.0f;
		get_derived_stats (CRITICAL_CHANCE).add_modifier (critical_chance_luck);

	}

	public void update_stats() {
		for ( int i = 0; i < _derived_stats.Length;i++) {
			_derived_stats [i].update();
		}
	}
}
