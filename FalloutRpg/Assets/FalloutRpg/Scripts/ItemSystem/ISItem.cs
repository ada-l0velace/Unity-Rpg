﻿using UnityEngine;
using System;
using System.Collections;
using FalloutRpg.ItemSystem.GUIEditor;


namespace FalloutRpg.ItemSystem {

	[System.Serializable]
	public abstract class ISItem : IISItem, IISGameObject {

		[SerializeField] private string _name;
		[SerializeField] private int _price;
		[SerializeField] private Sprite _icon;
		[SerializeField] private int _weight;
		[SerializeField] private ISRarity _rarity;
		[SerializeField] GameObject _prefab;
		ISItemEditorManager _editor;

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
			_editor = new ISItemEditorManager ();
		}

		public virtual void Clone (ISItem weapon) {
			Name = weapon.Name;
			Price = weapon.Price;
			Weight = weapon.Weight;
			Rarity = weapon.Rarity;
			Icon = weapon.Icon;
			Prefab = weapon.Prefab;
		}

		public virtual void OnGUI () {
			_editor.OnGUI (_name, _price, _weight, _icon, _rarity, _prefab);
		}

	}
}