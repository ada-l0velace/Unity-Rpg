using UnityEngine;
using System.Collections;

public interface IISWeapon {

	int MinDamage { get; set; }
	int MaxDamage { get; set; }
	int Attack ();
}
