﻿using UnityEngine;
using System.Collections;


namespace FalloutRpg.ItemSystem {

	[System.Serializable]
	public class ISItem : IISItem {

		[SerializeField] private string _name;
		[SerializeField] private string _value;
		[SerializeField] private Sprite _icon;
		[SerializeField] private int _weight;
		[SerializeField] private ISRarity _rarity;

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
		public string Value {
			get {
				return _value;
			}
			set {
				_value = value;
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

	}
}