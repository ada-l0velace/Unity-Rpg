using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace FalloutRpg.ItemSystem {

	public class ISRarityDatabase : ScriptableObjectDatabase <ISRarity> {
		public int GetIndex ( string name ) {
			return database.FindIndex ( a => a.Name==name);
		}
	}
}
