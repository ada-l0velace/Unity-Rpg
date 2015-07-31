using UnityEngine;
using UnityEditor;
using System.Collections;

namespace FalloutRpg.ItemSystem.Editor {
	public partial class ISItemDatabaseEditor  {
		Vector2 _scrollPos = Vector2.zero;
		int _listViewWidth = 200;
		int _gridIndex = -1;

		/// <summary>
		/// Displays all Items in the database.
		/// </summary>
		void ListView() {
			_scrollPos = GUILayout.BeginScrollView (_scrollPos, "Box", GUILayout.ExpandHeight(true), GUILayout.Width(_listViewWidth));
			GUILayout.Label ("List View");
			/*_gridIndex = GUILayout.SelectionGrid(_gridIndex, db.GetAllNames(), 1);
			*/
			for (int i = 0; i < db.Count; i++) {
				if(GUILayout.Button (db.Get(i).Name)){
					_gridIndex = i;
					tempWeapon = db.Get (i);
					togleNewWeapon = true;
					_state = DisplayState.DETAILS;
				}
				//EditorGUILayout.LabelField ();
			}
			GUILayout.EndScrollView ();
		}

	}
}
