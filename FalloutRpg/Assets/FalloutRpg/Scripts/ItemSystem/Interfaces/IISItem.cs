using UnityEngine;
using System.Collections;
using FalloutRpg.ItemSystem;

public interface IISItem  {
	// Name
	// Value - gold value
	// Icon
	// Weight
	// Rarity
	string Name { get; set; }
	int Price { get; set; }
	Sprite Icon { get; set; }
	int Weight { get; set; }
	ISRarity Rarity { get; set; }

	// Equip
	// Quest Item Flag
	// TakeDamage
}
