﻿using UnityEngine;
using System.Collections;


namespace FalloutRpg.ItemSystem {
	/// <summary>
	/// IS rarity.
	/// </summary>
	[System.Serializable]
	public class ISRarity : IISRarity {
		[SerializeField] string _name;
		[SerializeField] Sprite _icon;

		/// <summary>
		/// Initializes a new instance of the <see cref="ISRarity"/> class.
		/// </summary>
		public ISRarity(){
			_name = "Common";
			_icon = new Sprite ();
		}

		#region IISRarity implementation
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

		#endregion



	}
}