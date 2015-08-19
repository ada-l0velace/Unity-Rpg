using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace FalloutRpg.ItemSystem.GUIEditor {
	public class ISItemEditorManager {
		int _rarityIndex = 0;
		private ISRarityDatabase _rdb;
		private bool _ISRarityDbLoaded = false;
		String [] _options;

		public void OnGUI (string name,int price,int weight, Sprite icon, ISRarity rarity, GameObject prefab) {
			name = EditorGUILayout.TextField ("Name", name); 
			price = EditorGUILayout.IntField ("Price", price);
			weight = EditorGUILayout.IntField ("Weight", weight);
			DisplayIcon (icon);
			DisplayRarity (rarity);
			DisplayPrefab (prefab);
		}

		private void DisplayIcon(Sprite icon) {
			icon = EditorGUILayout.ObjectField ("Icon", icon, typeof(Sprite), false) as Sprite;
		}

		private void DisplayRarity(ISRarity rarity) {

			int itemIndex = 0;

			if (!_ISRarityDbLoaded) {
				LoadQualityDatabase ();
				return;
			}

			if (rarity != null)
				itemIndex = _rdb.GetIndex (rarity.Name);

			if (itemIndex == -1) {
				if(_rdb.Count == 0)
					return;
				itemIndex = 0;
			}
			_rarityIndex = EditorGUILayout.Popup ("Rarity", itemIndex, _options);
			rarity = _rdb.Get (_rarityIndex);
		}

		private void DisplayPrefab (GameObject prefab) {
			prefab = EditorGUILayout.ObjectField ("Prefab", prefab, typeof(GameObject), false) as GameObject;
		}

		private void LoadQualityDatabase() {
			string databaseName =  @"ISRarityDatabase.asset";
			string databasePath = @"Database";
			_rdb = ISRarityDatabase.GetDatabase<ISRarityDatabase> (databasePath, databaseName);
			_options = new string[_rdb.Count];
			for (int i = 0; i < _rdb.Count; i++) {
				_options [i] = _rdb.Get (i).Name;
			}
			_ISRarityDbLoaded = true;
		}
	}
}