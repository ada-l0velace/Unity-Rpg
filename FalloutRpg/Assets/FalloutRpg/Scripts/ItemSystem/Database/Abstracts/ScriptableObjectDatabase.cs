using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace FalloutRpg.ItemSystem {

	public abstract class ScriptableObjectDatabase<T> : ScriptableObject where T: class {
		[SerializeField]
		protected List<T> database  = new List<T>();

		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add (T item){
			database.Add (item);
			EditorUtility.SetDirty (this);
		}

		/// <summary>
		/// Inserts item on the specified index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="item">Item</param>
		public void Insert (int index, T item){
			database.Insert (index, item);
			EditorUtility.SetDirty (this);
		}

		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Remove (T item){
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
		public T Get(int index) {
			return database[index];
		}

		/// <summary>
		/// Replaces a item in the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		public void Replace(int index, T item) {
			database [index] = item;
			EditorUtility.SetDirty (this);
		}

		/// <summary>
		/// Creates the database according to its type U and returns it.
		/// </summary>
		/// <returns>The database.</returns>
		/// <param name="db_path">Db_path.</param>
		/// <param name="db_name">Db_name.</param>
		/// <typeparam name="U">The 1st type parameter.</typeparam>
		public static U GetDatabase <U> (string db_path, string db_name) where U : ScriptableObject {
			string db_full_path = @"Assets/" + db_path + "/" + db_name;

			U db = AssetDatabase.LoadAssetAtPath (db_full_path, typeof(U)) as U;
			if (db == null) {
				if (!AssetDatabase.IsValidFolder ("Assets/" + db_path))
					AssetDatabase.CreateFolder ("Assets", db_path);

				db = ScriptableObject.CreateInstance<U> ();
				AssetDatabase.CreateAsset (db, db_full_path);
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();
			}
			return db;
		}
	}
}
