using System;                                  // Basic types.
using System.Collections;                      // IEnumerable.
using System.Collections.Generic;              // IEnumerable<T>.
namespace PROG7312.POE.DataStructures
{
    public sealed class Queue<T> : IEnumerable<T> // FIFO built on our LinkedList<T>.
    {
        private readonly LinkedList<T> _list = new LinkedList<T>(); // No arrays/lists.

        public int Count => _list.Count;
        public bool IsEmpty() => _list.IsEmpty();

        public void Enqueue(T item) { _list.AddLast(item); }      // Add to back.
        public T Dequeue()                                        // Remove front.
        {
            if (_list.IsEmpty()) throw new InvalidOperationException("Queue is empty");
            return _list.RemoveFirst();
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
