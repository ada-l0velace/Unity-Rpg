using UnityEditor;
using UnityEngine;
using System.Collections;


namespace FalloutRpg.ItemSystem.Editor {

	public partial class ISRarityDatabaseEditor : EditorWindow {
		private ISRarityDatabase db;
		private Texture2D  _selected_texture;
		private Vector2 _scroll_pos;
		private int _selected_index;

		const int SPRITE_BUTTON_SIZE = 46;
		const string FILE_NAME = @"ISRarityDatabase.asset";
		const string DATABASE_NAME = @"Database";
		const string DATABASE_FULL_PATH = @"Assets/" + DATABASE_NAME + "/" + FILE_NAME; 

		[MenuItem("FalloutRpg/Database/Rarity Editor %#i")]

		/// <summary>
		/// Init this instance.
		/// </summary>
		public static void Init(){
			ISRarityDatabaseEditor window = EditorWindow.GetWindow<ISRarityDatabaseEditor> ();
			window.minSize = new Vector2 (400, 300);
			window.titleContent.text = "Rarity Database";
			window.Show ();
		}

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		void OnEnable() {
			db = ISRarityDatabase.GetDatabase<ISRarityDatabase> (DATABASE_NAME, FILE_NAME);
		}

		/// <summary>
		/// Raises the GU event.
		/// </summary>
		void OnGUI() {
			ListView ();
			bottom_bar ();
		}

		/// <summary>
		/// This is the bottom bar in the editor.
		/// </summary>
		void bottom_bar () {
			GUILayout.BeginHorizontal ("box", GUILayout.ExpandWidth (true));
			GUILayout.Label ("Rarities: " + db.Count);
			if (GUILayout.Button ("Add")) {
				db.Add (new ISRarity ());
			}
			GUILayout.EndHorizontal ();
		}

	}
}


