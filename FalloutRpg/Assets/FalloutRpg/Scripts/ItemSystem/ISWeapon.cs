using UnityEngine;
using System.Collections;

namespace FalloutRpg.ItemSystem {
	public class ISWeapon : ISBuff, IISWeapon, IISGameObject {

		[SerializeField]
		int _minDamage;
		[SerializeField]
		int _maxDamage;
		[SerializeField]

		/// <summary>
		/// Initializes a new instance of the <see cref="FalloutRpg.ItemSystem.ISWeapon"/> class.
		/// </summary>
		/// <param name="minDamage">Minimum damage.</param>
		/// <param name="maxDamage">Max damage.</param>
		public ISWeapon(int minDamage, int maxDamage) {
			MinDamage = minDamage;
			MaxDamage = maxDamage;
		}

		#region IISWeapon implementation

		/// <summary>
		/// Calculates the damage the weapon will do.
		/// </summary>
		public int Attack ()
		{
			throw new System.NotImplementedException ();
		}

		/// <summary>
		/// Gets or sets the minimum damage.
		/// </summary>
		/// <value>The minimum damage.</value>
		public int MinDamage {
			get {
				return _minDamage;
			}
			set {
				_minDamage = value;
			}
		}

		/// <summary>
		/// Gets or sets the max damage.
		/// </summary>
		/// <value>The max damage.</value>
		public int MaxDamage {
			get {
				return _maxDamage;
			}
			set {
				_maxDamage = value;
			}
		}

		#endregion



	}
}
