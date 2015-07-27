using UnityEditor;
using UnityEngine;
using System.Collections;


namespace FalloutRpg.ItemSystem.Editor {

	public class ISRarityDatabaseEditor : EditorWindow {
		ISRarityDatabase db;
		ISRarity selected_item;
		Texture2D  selected_texture;

		const int SPRITE_BUTTON_SIZE = 92;
		const string FILE_NAME = @"ISRarityDatabase.asset";
		const string DATABASE_FOLDER_NAME = @"Database";
		const string DATABASE_PATH = @"Assets/" + DATABASE_FOLDER_NAME + "/" + FILE_NAME; 

		[MenuItem("FalloutRpg/Database/Quality Editor %#i")]

		public static void Init(){
			ISRarityDatabaseEditor window = EditorWindow.GetWindow<ISRarityDatabaseEditor> ();
			window.minSize = new Vector2 (400, 300);
			window.titleContent.text = "Rarity Database";
			window.Show ();
		}

		void OnEnable() {
			db = AssetDatabase.LoadAssetAtPath (DATABASE_PATH, typeof(ISRarityDatabase)) as ISRarityDatabase;
			if (db == null) {
				if (!AssetDatabase.IsValidFolder ("Assets/" + DATABASE_FOLDER_NAME))
					AssetDatabase.CreateFolder ("Assets", DATABASE_FOLDER_NAME);
				
				db = ScriptableObject.CreateInstance<ISRarityDatabase> ();
				AssetDatabase.CreateAsset (db, DATABASE_PATH);
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();
			}

			selected_item = new ISRarity ();
		}

		void OnGUI() {
			add_rarity_to_database();
		}

		void add_rarity_to_database() {
			selected_item.Name = EditorGUILayout.TextField ("Name:", selected_item.Name);
			if (selected_item.Icon)
				selected_texture = selected_item.Icon.texture;
			else
				selected_texture = null;

			if(GUILayout.Button (selected_texture, GUILayout.Width(SPRITE_BUTTON_SIZE), GUILayout.Height(SPRITE_BUTTON_SIZE))) {
				int controler_id = EditorGUIUtility.GetControlID(FocusType.Passive);
				EditorGUIUtility.ShowObjectPicker<Sprite>(null, true, null, controler_id);
			}

			string command_name = Event.current.commandName;
			if (command_name == "ObjectSelectorUpdated") {
				selected_item.Icon = (Sprite)EditorGUIUtility.GetObjectPickerObject ();
				Repaint ();
			}

			if (GUILayout.Button ("Save")) {
				if (selected_item == null)
					return;
				db.database.Add (selected_item);
				selected_item = new ISRarity ();
			} 
		}
	}
}


