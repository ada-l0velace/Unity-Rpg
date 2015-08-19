using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FalloutRpg.ItemSystem;

public class Inventory : IInventory {
	List <ISItem> _items;

	#region IInventory implementation

	public List<ISItem> Items {
		get {
			return _items;
		}
	}

	#endregion

	/// <summary>
	/// Initializes a new instance of the <see cref="Inventory"/> class.
	/// </summary>
	public Inventory() {
	}

	/// <summary>
	/// Add the specified item to the Inventory.
	/// </summary>
	/// <param name="item">Item.</param>
	public void Add (ISItem item) {
		_items.Add (item);
	}

	/// <summary>
	/// Remove the specified item from the Inventory.
	/// </summary>
	/// <param name="item">Item.</param>
	public void Remove (ISItem item) {
		_items.Remove (item);
	}

}
