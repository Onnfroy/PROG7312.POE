using System;
using PROG7312.POE.DataStructures; // your LinkedList<T>

namespace PROG7312.POE.DataStructures
{
    public sealed class MinPriorityQueue<T>
    {
        private readonly LinkedList<T> _list = new LinkedList<T>();
        private readonly Func<T, T, int> _compare; // return <0 if a<b

        public MinPriorityQueue(Func<T, T, int> comparer)
        {
            _compare = comparer;
        }

        public bool IsEmpty() => _list.IsEmpty();

        public int Count => _list.Count;

        public void Enqueue(T item)
        {
            // Insert in sorted order (ascending)
            if (_list.IsEmpty()) { _list.AddLast(item); return; }

            // We don't have AddBefore; build a new list with item placed at first position where item <= current
            var newList = new LinkedList<T>();
            bool inserted = false;
            foreach (var existing in _list)
            {
                if (!inserted && _compare(item, existing) <= 0)
                {
                    newList.AddLast(item);
                    inserted = true;
                }
                newList.AddLast(existing);
            }
            if (!inserted) newList.AddLast(item);
            // Replace underlying list by copying back:
            _listCopyFrom(newList);
        }

        public T PeekMin()
        {
            if (_list.IsEmpty()) throw new InvalidOperationException("Empty queue");
            // Peek = first element; we don't have First, so RemoveFirst then reinsert at head.
            T first = _list.RemoveFirst();
            // Put it back at head by rebuilding
            var temp = new LinkedList<T>();
            temp.AddLast(first);
            foreach (var x in _list) temp.AddLast(x);
            _listCopyFrom(temp);
            return first;
        }

        public T DequeueMin()
        {
            if (_list.IsEmpty()) throw new InvalidOperationException("Empty queue");
            return _list.RemoveFirst();
        }

        private void _listCopyFrom(LinkedList<T> src)
        {
            // Clear _list by draining it
            while (!_list.IsEmpty()) _list.RemoveFirst();
            foreach (var x in src) _list.AddLast(x);
        }
    }
}