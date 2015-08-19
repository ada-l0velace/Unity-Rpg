using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FibonacciHeap <T> where T : class, IFibonacciHeapItem<T>  {
	private static readonly double _oneOverLogPhi = 1.0/Math.Log((1.0 + Math.Sqrt(5.0))/2.0);
	T _head;
	T [] _trees;
	int _maxNodes, _maxTrees, _n, _value;
	int _keyComps;


	public FibonacciHeap( int max_heap_size) {
		_maxTrees = System.Convert.ToInt32(1.0 + 1.44 * Mathf.Log(max_heap_size)/ Mathf.Log(2.0f));
		_maxNodes = max_heap_size;
		_trees = new T [_maxTrees];
		//_nodes = new T [_maxNodes];
		_value = 0;
		_keyComps = 0;
	}

	public void Add (T node) {
		node.Left = node.Right = node;
		//node.HeapIndex = vertex_no;
		//_head [vertex_no] = node;
		if (_head != null)
			Merge (node);
		else
			_head = node;
		_n++;
	}

	public void Merge(T node) {
		(_head.Left).Right = node;
		node.Right	= _head;
		node.Left = _head.Left;
		_head.Left = node;
		if (node.Key < _head.Key)
			_head = node;
	}

	public void Display() {
		T p = _head;
		if (p == null) {
			Debug.Log ("Empty Heap");
			return;
		}
		Debug.Log ("The root nodes of Heap are: ");
		do {
			Debug.Log(p.Key);
			Debug.Log("-------------");
			for (T child = p.Child; child != null; child = child.Child) {
				Debug.Log(child.Key);
				if(child.Left != null)
					Debug.Log(child.Left.Key);
			}
			Debug.Log("-------------");
			p = p.Right;
			if (p != _head)
				Debug.Log("-->");

		}while(p != _head && p.Right != null);
	}

	public int count () {
		return _n;
	}
	public T remove_first () {
		T minNode = _head;
		if (minNode != null) {
			int numKids = minNode.Rank;
			T oldMinChild = minNode.Child;

			while (numKids > 0) {
				T tempRight = oldMinChild.Right;

				// remove oldMinChild from child list
				oldMinChild.Left.Right = oldMinChild.Right;
				oldMinChild.Right.Left = oldMinChild.Left;

				// add oldMinChild to root list of heap
				oldMinChild.Left = _head;
				oldMinChild.Right = _head.Right;
				_head.Right = oldMinChild;
				oldMinChild.Right.Left = oldMinChild;

				// set parent[oldMinChild] to null
				oldMinChild.Parent = null;
				oldMinChild = tempRight;
				numKids--;
			}

			// remove minNode from root list of heap
			minNode.Left.Right = minNode.Right;
			minNode.Right.Left = minNode.Left;

			if (minNode == minNode.Right)
			{
				_head = null;
			}
			else
			{
				_head = minNode.Right;
				Consolidate();
			}

			// decrement size of heap
			_n--;

		}

		return _head;
	}

	protected void Consolidate()
	{
		int arraySize = ((int) Math.Floor(Math.Log(_n)*_oneOverLogPhi)) + 1;

		var array = new List<T>(arraySize);

		// Initialize degree array
		for (var i = 0; i < arraySize; i++)
		{
			array.Add(null);
		}

		// Find the number of root nodes.
		var numRoots = 0;
		T x = _head;

		if (x != null)
		{
			numRoots++;
			x = x.Right;

			while (x != _head)
			{
				numRoots++;
				x = x.Right;
			}
		}

		// For each node in root list do...
		while (numRoots > 0)
		{
			// Access this node's degree..
			int d = x.Rank;
			T next = x.Right;

			// ..and see if there's another of the same degree.
			for (;;)
			{
				T y = array[d];
				if (y == null)
				{
					// Nope.
					break;
				}

				// There is, make one of the nodes a child of the other.
				// Do this based on the key value.
				if (x.Key > y.Key)
				{
					T temp = y;
					y = x;
					x = temp;
				}

				// FibonacciHeapNode<T> newChild disappears from root list.
				Link(y, x);

				// We've handled this degree, go to next one.
				array[d] = null;
				d++;
			}

			// Save this node for later when we might encounter another
			// of the same degree.
			array[d] = x;

			// Move forward through list.
			x = next;
			numRoots--;
		}

		// Set min to null (effectively losing the root list) and
		// reconstruct the root list from the array entries in array[].
		_head = null;

		for (var i = 0; i < arraySize; i++)
		{
			T y = array[i];
			if (y == null)
			{
				continue;
			}

			// We've got a live one, add it to root list.
			if (_head != null)
			{
				// First remove node from root list.
				y.Left.Right = y.Right;
				y.Right.Left = y.Left;

				// Now add to root list, again.
				y.Left = _head;
				y.Right = _head.Right;
				_head.Right = y;
				y.Right.Left = y;

				// Check if this is a new min.
				if (y.Key < _head.Key)
				{
					_head = y;
				}
			}
			else
			{
				_head = y;
			}
		}
	}

	/// <summary>
	/// Makes newChild a child of Node newParent.
	/// O(1)
	/// </summary>
	protected void Link(T newChild, T newParent)
	{
		// remove newChild from root list of heap
		newChild.Left.Right = newChild.Right;
		newChild.Right.Left = newChild.Left;

		// make newChild a child of newParent
		newChild.Parent = newParent;

		if (newParent.Child == null)
		{
			newParent.Child = newChild;
			newChild.Right = newChild;
			newChild.Left = newChild;
		}
		else
		{
			newChild.Left = newParent.Child;
			newChild.Right = newParent.Child.Right;
			newParent.Child.Right = newChild;
			newChild.Right.Left = newChild;
		}

		// increase degree[newParent]
		newParent.Rank++;

		// set mark[newChild] false
		newChild.Marked = false;
	}
}

public interface IFibonacciHeapItem<T> {
	int HeapIndex{
		get;
		set;
	}
	T Parent {
		get;
		set;
	}
	T Child {
		get;
		set;
	}
	T Right {
		get;
		set;
	}
	T Left {
		get;
		set;
	}
	int Rank {
		get;
		set;
	}
	int Key {
		get;
		set;
	}
	bool Marked {
		get;
		set;
	}
}