using UnityEngine;
using UnityEditor;
using System.Collections;

namespace FalloutRpg.ItemSystem.Editor {
	public partial class ISItemDatabaseEditor : EditorWindow {
		ISWeaponDatabase db;
		const string FILE_NAME = @"ISWeaponDatabase.asset";
		const string DATABASE_NAME = @"Database";
		const string DATABASE_FULL_PATH = @"Assets/" + DATABASE_NAME + "/" + FILE_NAME; 

		[MenuItem("FalloutRpg/Database/Item Editor %#d")]

		/// <summary>
		/// Init this instance.
		/// </summary>
		public static void Init(){
			ISItemDatabaseEditor window = EditorWindow.GetWindow<ISItemDatabaseEditor> ();
			window.minSize = new Vector2 (800, 600);
			window.titleContent.text = "Item System";
			window.Show ();
		}

		void OnEnable() {
			db = ISWeaponDatabase.GetDatabase<ISWeaponDatabase> (DATABASE_NAME, FILE_NAME);
		}

		void OnGUI()
		{
			TopBar ();
			ListView ();
		}
	}
}