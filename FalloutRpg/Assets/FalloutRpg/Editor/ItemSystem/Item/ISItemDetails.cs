using UnityEngine;
using System.Collections;

namespace FalloutRpg.ItemSystem.Editor {
	public partial class ISItemDatabaseEditor  {

		void ItemDetails () {
			GUILayout.BeginHorizontal ("Box", GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));
			GUILayout.Label ("Detail View");
			GUILayout.EndHorizontal ();
		}

	}
}
