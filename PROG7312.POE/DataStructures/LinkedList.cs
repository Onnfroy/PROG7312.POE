using System;                      // Basic types.
using System.Collections;          // For IEnumerable (non-generic).
using System.Collections.Generic;  // For IEnumerable<T> only (not List/Array).

namespace PROG7312.POE.DataStructures
{
    public sealed class LinkedListNode<T>     // Single node in our chain.
    {
        public T Value;                       // Payload we store.
        public LinkedListNode<T> Next;        // Link to next node.
        public LinkedListNode(T value) { Value = value; Next = null; }
    }

    public sealed class LinkedList<T> : IEnumerable<T> // Our singly linked list.
    {
        private LinkedListNode<T> _head;      // First node.
        private LinkedListNode<T> _tail;      // Last node for O(1) append.
        public int Count { get; private set; } // Item count.

        public LinkedList() { _head = _tail = null; Count = 0; }

        public void AddLast(T item)           // Append to end.
        {
            var node = new LinkedListNode<T>(item);
            if (_head == null) { _head = _tail = node; }
            else { _tail.Next = node; _tail = node; }
            Count++;
        }

        public T RemoveFirst()                // Pop from front.
        {
            if (_head == null) throw new InvalidOperationException("List is empty");
            var value = _head.Value;
            _head = _head.Next;
            if (_head == null) _tail = null;
            Count--;
            return value;
        }

        public bool IsEmpty() => Count == 0;  // Helper.

        public IEnumerator<T> GetEnumerator() // foreach support.
        {
            var current = _head;
            while (current != null) { yield return current.Value; current = current.Next; }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}