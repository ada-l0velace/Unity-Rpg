using UnityEngine;
using UnityEditor;
using System.Collections;

namespace FalloutRpg.ItemSystem.Editor {
	public partial class ISItemDatabaseEditor  {
		Vector2 _scrollPos = Vector2.zero;
		int _listViewWidth = 200;

		/// <summary>
		/// Displays all Items in the database.
		/// </summary>
		void ListView() {
			_scrollPos = GUILayout.BeginScrollView (_scrollPos, "Box", GUILayout.ExpandHeight(true), GUILayout.Width(_listViewWidth));
			GUILayout.EndScrollView ();
		}

	}
}
