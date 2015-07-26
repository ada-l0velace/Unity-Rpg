using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T> {

	T[] _items;
	int _current_item_count;

	public Heap(int max_heap_size) {
		_items = new T [max_heap_size];
	}

	public void add(T item) {
		item.HeapIndex = _current_item_count;
		_items[_current_item_count] = item;
		sort_up(item);
		_current_item_count++;
	}

	public T remove_first() {
		T first_item = _items[0];
		_current_item_count--;
		_items[0] = _items[_current_item_count];
		_items[0].HeapIndex = 0;
		sort_down(_items [0]);
		return first_item;

	}
	
	public bool contains(T item) {
		return Equals(_items[item.HeapIndex],item);
	}

	public void update_item (T item) {
		sort_up(item);
	}

	public int count {
		get {
			return _current_item_count;
		}
	}

	void sort_down (T item) {
		while (true) {
			int child_index_left = item.HeapIndex * 2 + 1;
			int child_index_right = item.HeapIndex * 2 + 2;
			int swap_index = 0;

			if (child_index_left < _current_item_count) {
				swap_index = child_index_left;

				if (child_index_right < _current_item_count) {
					if(_items [child_index_left].CompareTo(_items [child_index_right]) < 0) {
						swap_index = child_index_right;
					}
				}

				if(item.CompareTo(_items [swap_index]) < 0)
					swap(item,_items [swap_index]);
				else
					return;
			}
			else
				return;

		}
	}

	void sort_up (T item) {
		int parent_index = (item.HeapIndex -1) / 2;
		while( true) {
			T parent_item = _items [parent_index];
			if (item.CompareTo(parent_item) > 0) {
				swap(item,parent_item);
			}
			else
				break;
			parent_index = (item.HeapIndex -1) / 2;
		}
	}

	void swap(T item_a, T item_b) {
		int temp_a_index = item_a.HeapIndex;
		_items[item_a.HeapIndex] = item_b;
		_items[item_b.HeapIndex] = item_a;
		item_a.HeapIndex = item_b.HeapIndex;
		item_b.HeapIndex = temp_a_index;
	}

}

public interface IHeapItem<T> : IComparable<T> {
	int HeapIndex{
		get;
		set;
	}
}