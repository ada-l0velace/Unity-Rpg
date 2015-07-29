using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FalloutRpg.ItemSystem {

	[System.Serializable]
	public class ISBuff : IISBuff {

		[SerializeField]
		private BaseStat _stat;
		[SerializeField]
		private int _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="FalloutRpg.ItemSystem.ISBuff"/> class.
		/// </summary>
		public ISBuff() {
			_stat = null;
			_value = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FalloutRpg.ItemSystem.ISBuff"/> class.
		/// </summary>
		/// <param name="stat">Stat.</param>
		/// <param name="value">Value.</param>
		public ISBuff(BaseStat stat, int value) {
			_stat = stat;
			_value = value;
		}

		#region IISBuff implementation

		/// <summary>
		/// Gets or sets the stat.
		/// </summary>
		/// <value>The stat.</value>
		public BaseStat Stat {
			get {
				return _stat;
			}
			set {
				_stat = value;
			}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public int Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}

		#endregion


	}
}