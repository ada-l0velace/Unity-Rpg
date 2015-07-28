using UnityEditor;
using UnityEngine;
using System.Collections;


namespace FalloutRpg.ItemSystem.Editor {

	public partial class ISRarityDatabaseEditor : EditorWindow {
		private ISRarityDatabase db;
		private ISRarity _selected_item;
		private Texture2D  _selected_texture;
		private Vector2 _scroll_pos;
		private int _selected_index;

		const int SPRITE_BUTTON_SIZE = 46;
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

			_selected_item = new ISRarity ();
		}

		void OnGUI() {
			ListView ();
			add_rarity_to_database();
		}

		void add_rarity_to_database() {
			_selected_item.Name = EditorGUILayout.TextField ("Name:", _selected_item.Name);
			if (_selected_item.Icon)
				_selected_texture = _selected_item.Icon.texture;
			else
				_selected_texture = null;

			if(GUILayout.Button (_selected_texture, GUILayout.Width(SPRITE_BUTTON_SIZE), GUILayout.Height(SPRITE_BUTTON_SIZE))) {
				int controler_id = EditorGUIUtility.GetControlID(FocusType.Passive);
				EditorGUIUtility.ShowObjectPicker<Sprite>(null, true, null, controler_id);
			}

			string command_name = Event.current.commandName;
			if (command_name == "ObjectSelectorUpdated") {
				_selected_item.Icon = (Sprite)EditorGUIUtility.GetObjectPickerObject ();
				Repaint ();
			}

			if (GUILayout.Button ("Save")) {
				if (_selected_item == null)
					return;
				if (_selected_item.Name == "")
					return;
				db.Add (_selected_item);
				_selected_item = new ISRarity ();
			} 
		}
	}
}


