using UnityEngine;
using UnityEditor;
using System.Collections;

namespace FalloutRpg.ItemSystem.Editor {

	public partial class ISRarityDatabaseEditor  {

		/// <summary>
		/// Lists all of the stored rarities in the database.
		/// </summary>
		void ListView() {
			_scroll_pos = EditorGUILayout.BeginScrollView (_scroll_pos, GUILayout.ExpandHeight(true));
			DisplayRarities ();
			EditorGUILayout.EndScrollView ();
		}

		/// <summary>
		/// Displays the rarities.
		/// </summary>
		void DisplayRarities() {
			for (int i = 0; i < db.Count; i++) {
				GUILayout.BeginHorizontal ("box");
					if (db.Get (i).Icon)
						_selected_texture = db.Get (i).Icon.texture;
					else
						_selected_texture = null;

					if(GUILayout.Button (_selected_texture, GUILayout.Width (SPRITE_BUTTON_SIZE), GUILayout.Height (SPRITE_BUTTON_SIZE))) {
						int controler_id = EditorGUIUtility.GetControlID(FocusType.Passive);
						EditorGUIUtility.ShowObjectPicker<Sprite>(null, true, null, controler_id);
						_selected_index = i;
					}

					string command_name = Event.current.commandName;
					if (command_name == "ObjectSelectorUpdated" && _selected_index != -1) {
						db.Get (_selected_index).Icon = (Sprite)EditorGUIUtility.GetObjectPickerObject ();
						Repaint ();
					}
					if (command_name == "ObjectSelectorClosed") 
						_selected_index = -1;
					
					GUILayout.BeginVertical ("box");
						db.Get (i).Name = GUILayout.TextField (db.Get (i).Name);
						GUILayout.BeginHorizontal ();
							if (GUILayout.Button ("Remove")) {
								if (EditorUtility.DisplayDialog ("Delete Quality", 
								                                 "Are you sure that you want to delete " + db.Get (i).Name + " from the database?", 
								                                 "Delete", 
								                                 "Cancel")) {
									db.Remove (i);
								}
							}
						GUILayout.EndHorizontal();
					GUILayout.EndVertical ();
				GUILayout.EndHorizontal();
			}
		}
	}
}
