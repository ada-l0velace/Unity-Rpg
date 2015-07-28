using UnityEngine;
using System.Collections;


namespace FalloutRpg.ItemSystem {
	/// <summary>
	/// IS rarity.
	/// </summary>
	[System.Serializable]
	public class ISRarity : IISRarity {
		[SerializeField] 
		private string _name;
		[SerializeField] 
		private Sprite _icon;

		/// <summary>
		/// Initializes a new instance of the <see cref="ISRarity"/> class.
		/// </summary>
		public ISRarity(){
			_name = "Enter a name here!";
			_icon = new Sprite ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FalloutRpg.ItemSystem.ISRarity"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="icon">Icon.</param>
		public ISRarity( string name, Sprite icon){
			_name = name;
			_icon = icon;
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