using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FalloutRpg.ItemSystem {
	public class ISRarityDatabase : ScriptableObject {
		//[SerializeField] 
		public List<ISRarity> database  = new List<ISRarity>();
	}
}
