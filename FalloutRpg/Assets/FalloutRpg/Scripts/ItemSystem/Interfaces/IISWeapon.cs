using UnityEngine;
using System.Collections;

public interface IISWeapon {

	int MinDamage { get; set; }
	int MaxDamage { get; set; }
	int AoeDamage { get; set; }
	int Range { get; set; }
	WeaponType EWeaponType { get; set; }
	DamageType EDamageType { get; set; }
	int Attack ();
}

public enum WeaponType {
	Knife,
	Club,
	Hammer,
	Spear,
	Knuckle,
	Glove,
	Pistol,
	SMG,
	Rifle,
	Shotgun,
	Throwing,
	BigGun,
	Explosive
}

public enum DamageType {
	Normal,
	Laser,
	Fire,
	Plasma,
	Explosive,
	Emp,
	Electrical
}
