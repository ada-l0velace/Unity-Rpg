using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace FalloutRpg.ItemSystem {

	public class ISRarityDatabase : ScriptableObject {
		private List<ISRarity> database  = new List<ISRarity>();

		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add (ISRarity item){
			database.Add (item);
			EditorUtility.SetDirty (this);
		}

		/// <summary>
		/// Inserts item on the specified index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="item">Item</param>
		public void Insert (int index, ISRarity item){
			database.Insert (index, item);
			EditorUtility.SetDirty (this);
		}

		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Remove (ISRarity item){
			database.Remove (item);
			EditorUtility.SetDirty (this);
		}

		/// <summary>
		/// Removes a item in the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public void Remove (int index) {
			database.RemoveAt (index);
			EditorUtility.SetDirty (this);
		}

		/// <summary>
		/// Gets number of items in the database.
		/// </summary>
		/// <value>The count.</value>
		public int Count {
			get{ return database.Count; }
		}

		/// <summary>
		/// Get the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public ISRarity Get(int index) {
			return database[index];
		}

		/// <summary>
		/// Replaces a item in the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		public void Replace(int index, ISRarity item) {
			database [index] = item;
			EditorUtility.SetDirty (this);
		}
	}
}
