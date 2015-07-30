using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FalloutRpg.ItemSystem {

	[System.Serializable]
	public abstract class ISBuffItem : ISItem {

		[SerializeField] private List <ISBuff> _buffs;

		#region Set and Geters
		/// <summary>
		/// Gets or sets the buffs.
		/// </summary>
		/// <value>The buffs.</value>
		public List <ISBuff> Buffs {
			get { return _buffs; }
			set { _buffs = value; }
		}

		#endregion

		/// <summary>
		/// Add the specified buff.
		/// </summary>
		/// <param name="buff">Buff.</param>
		public void Add(ISBuff buff) {
			Buffs.Add (buff);
		}

		/// <summary>
		/// Remove the specified buff.
		/// </summary>
		/// <param name="buff">Buff.</param>
		public void Remove(ISBuff buff) {
			Buffs.Remove (buff);
		}

	}
}