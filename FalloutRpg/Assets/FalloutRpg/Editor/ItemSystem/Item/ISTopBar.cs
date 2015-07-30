using UnityEngine;
using System.Collections;

namespace FalloutRpg.ItemSystem.Editor {
	public partial class ISItemDatabaseEditor  {
		public int selGridInt = 0;
		public string[] selStrings = new string[] {"Weapons", "Armor", "Potions", "About"};
		void TopBar () {

			selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 4);

		}

	}
}
