using UnityEngine;
using UnityEditor;
using System;
using System.Collections;


namespace FalloutRpg.ItemSystem {

	[System.Serializable]
	public abstract class ISItem : IISItem, IISGameObject {

		[SerializeField] private string _name;
		[SerializeField] private int _price;
		[SerializeField] private Sprite _icon;
		[SerializeField] private int _weight;
		[SerializeField] private ISRarity _rarity;
		[SerializeField] GameObject _prefab;
		private int rarityIndex = 0;
		private String [] options;
		private ISRarityDatabase rdb;


		#region IISObject implementation
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public int Price {
			get {
				return _price;
			}
			set {
				_price = value;
			}
		}
		/// <summary>
		/// Gets or sets the icon.
		/// </summary>
		/// <value>The icon.</value>
		public Sprite Icon {
			get {
				return _icon;
			}
			set {
				_icon = value;
			}
		}
		/// <summary>
		/// Gets or sets the weight.
		/// </summary>
		/// <value>The weight.</value>
		public int Weight {
			get {
				return _weight;
			}
			set {
				_weight = value;
			}
		}
		/// <summary>
		/// Gets or sets the rarity.
		/// </summary>
		/// <value>The rarity.</value>
		public ISRarity Rarity {
			get {
				return _rarity;
			}
			set {
				_rarity = value;
			}
		}

		#endregion

		#region IISGameObject implementation
		/// <summary>
		/// Gets or sets the prefab.
		/// </summary>
		/// <value>The prefab.</value>
		public GameObject Prefab {
			get {
				return _prefab;
			}
			set {
				_prefab = value;
			}
		}

		#endregion
            
		public ISItem() {
			string databaseName =  @"ISRarityDatabase.asset";
			string databasePath = @"Database";
			rdb = ISRarityDatabase.GetDatabase<ISRarityDatabase> (databasePath, databaseName);
			options = new string[rdb.Count];
			for (int i = 0; i < rdb.Count; i++) {
				options [i] = rdb.Get (i).Name;
			}
		}

		public virtual void OnGUI () {
			_name = EditorGUILayout.TextField ("Name", _name); 
			_price = Convert.ToInt32(EditorGUILayout.TextField ("Price", _price.ToString()));
			_weight = Convert.ToInt32(EditorGUILayout.TextField ("Weight", _weight.ToString()));
			DisplayIcon ();
			DisplayRarity ();
			DisplayPrefab ();
		}

		public void DisplayIcon() {
			_icon = EditorGUILayout.ObjectField ("Icon", _icon, typeof(Sprite), false) as Sprite;
		}

		public void DisplayRarity() {
			rarityIndex = EditorGUILayout.Popup ("Rarity", rarityIndex, options);
			_rarity = rdb.Get (rarityIndex);
		}

		public void DisplayPrefab () {
			_prefab = EditorGUILayout.ObjectField ("Prefab", _prefab, typeof(GameObject), false) as GameObject;
		}

	}
}