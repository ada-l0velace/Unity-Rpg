using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FalloutRpg.ItemSystem;

public interface IInventory {
	List <ISItem> Items { get; }

}
