using UnityEngine;
using System.Collections;

namespace FalloutRpg.ItemSystem {

	public class ISWeaponDatabase : ScriptableObjectDatabase <ISWeapon> {

		public string [] GetAllNames() {
			string [] names = new string [this.Count];
			for (int i = 0; i < this.Count; i++) {
				names [i] = this.Get (i).Name;
			}
			return names;
		}

	}
}
