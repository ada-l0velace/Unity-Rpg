using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FalloutRpg.ItemSystem {

	[System.Serializable]
	public class ISWeapon : ISBuffItem, IISWeapon {

		[SerializeField] private int _minDamage;
		[SerializeField] private int _maxDamage;
		[SerializeField] private int _aoeDamage;
		[SerializeField] private int _range;
		[SerializeField] private WeaponType _weaponType;
		[SerializeField] private DamageType _damageType;

		/// <summary>
		/// Initializes a new instance of the <see cref="FalloutRpg.ItemSystem.ISWeapon"/> class.
		/// </summary>
		public ISWeapon() : base ()  {
			MinDamage = 0;
			MaxDamage = 0;
			AoeDamage = 0;
			EWeaponType = WeaponType.Knife;
			EDamageType = DamageType.Normal;
			Range = 0;
			PBuffsL = new List <ISBuff<StatName>>();
			//Prefab = new GameObject ();
		}

		public ISWeapon( ISWeapon weapon) {
			Clone (weapon);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FalloutRpg.ItemSystem.ISWeapon"/> class.
		/// </summary>
		/// <param name="minDamage">Minimum damage.</param>
		/// <param name="maxDamage">Max damage.</param>
		/// <param name="aoeDamage">Aoe damage.</param>
		/// <param name="weaponType">Weapon type.</param>
		/// <param name="damageType">Damage type.</param>
		/// <param name="range">Range.</param>
		/// <param name="prefab">Prefab.</param>
		public ISWeapon(int minDamage, int maxDamage,int aoeDamage, WeaponType weaponType, DamageType damageType, int range) : base ()  {
			MinDamage = minDamage;
			MaxDamage = maxDamage;
			AoeDamage = aoeDamage;
			EWeaponType = weaponType;
			EDamageType = damageType;
			Range = range;

		}

		public new void Clone (ISWeapon weapon) {
			base.Clone (weapon);
			MinDamage = weapon.MinDamage;
			MaxDamage = weapon.MaxDamage;
			AoeDamage = weapon.AoeDamage;
			EWeaponType = weapon.EWeaponType;
			EDamageType = weapon.EDamageType;
			Range = weapon.Range;
			PBuffsL = weapon.PBuffsL;
			DBuffsL = weapon.DBuffsL;

		}

		#region IISWeapon implementation

		/// <summary>
		/// Calculates the damage the weapon will do.
		/// </summary>
		public int Attack ()
		{
			throw new System.NotImplementedException ();
		}

		/// <summary>
		/// Gets or sets the minimum damage.
		/// </summary>
		/// <value>The minimum damage.</value>
		public int MinDamage {
			get {
				return _minDamage;
			}
			set {
				_minDamage = value;
			}
		}

		/// <summary>
		/// Gets or sets the max damage.
		/// </summary>
		/// <value>The max damage.</value>
		public int MaxDamage {
			get {
				return _maxDamage;
			}
			set {
				_maxDamage = value;
			}
		}

		/// <summary>
		/// Gets or sets the range.
		/// </summary>
		/// <value>The range.</value>
		public int Range {
			get {
				return _range;
			}
			set {
				_range = value;
			}
		}

		/// <summary>
		/// Gets or sets the aoe damage.
		/// </summary>
		/// <value>The aoe damage.</value>
		public int AoeDamage {
			get {
				return _aoeDamage;
			}
			set {
				_aoeDamage = value;
			}
		}

		/// <summary>
		/// Gets or sets the type of the weapon.
		/// </summary>
		/// <value>The type of the E weapon.</value>
		public WeaponType EWeaponType {
			get {
				return _weaponType;
			}
			set {
				_weaponType = value;
			}
		}

		/// <summary>
		/// Gets or sets the type of the damage.
		/// </summary>
		/// <value>The type of the E damage.</value>
		public DamageType EDamageType {
			get {
				return _damageType;
			}
			set {
				_damageType = value;
			}
		}

		#endregion

		public override void OnGUI() {
			GUILayout.BeginVertical ();
			base.OnGUI ();
			DisplayWeaponType ();
			DisplayDamageType ();
			DisplayBuffs ();
			//GUILayout.EndVertical();
			//GUILayout.BeginVertical ();
			_minDamage = Convert.ToInt32(EditorGUILayout.TextField ("Min Damage", _minDamage.ToString()));
			_maxDamage = Convert.ToInt32(EditorGUILayout.TextField ("Max Damage", _maxDamage.ToString()));
			_aoeDamage = Convert.ToInt32(EditorGUILayout.TextField ("Aoe Damage", _aoeDamage.ToString()));

			GUILayout.EndVertical();
		}

		public void DisplayWeaponType() {
			_weaponType = (WeaponType) EditorGUILayout.EnumPopup ("Weapon Type", _weaponType);
		}

		public void DisplayDamageType() {
			_damageType = (DamageType)EditorGUILayout.EnumPopup ("Damage Type", _damageType);
		}

	}

}
