using UnityEngine;
using System.Collections;

namespace FalloutRpg.ItemSystem.Editor {
	public partial class ISItemDatabaseEditor  {

		void TopBar () {
			GUILayout.BeginHorizontal ("Box", GUILayout.ExpandWidth (true));
			GUILayout.Button ("Weapons");
			GUILayout.Button ("Armor");
			GUILayout.Button ("Potions");
			GUILayout.Button ("About");
			GUILayout.EndHorizontal ();
		}

	}
}
