using System;
using System.Collections;
using System.Collections.Generic;

namespace PROG7312.POE.DataStructures
{
    // Simple BST keyed by string (we'll use request Id.ToString()).
    public sealed class BSTValuePair<TValue>
    {
        public string Key;     // comparison key (string GUID)
        public TValue Value;   // payload
        public BSTValuePair(string key, TValue value) { Key = key; Value = value; }
    }

    internal sealed class BSTNode<TValue>
    {
        public BSTValuePair<TValue> Item;
        public BSTNode<TValue> Left;
        public BSTNode<TValue> Right;
        public BSTNode(BSTValuePair<TValue> item) { Item = item; }
    }

    public sealed class BST<TValue> : IEnumerable<BSTValuePair<TValue>>
    {
        private BSTNode<TValue> _root;

        public void Insert(string key, TValue value)
        {
            var node = new BSTNode<TValue>(new BSTValuePair<TValue>(key, value));
            if (_root == null) { _root = node; return; }
            var cur = _root;
            while (true)
            {
                int cmp = string.CompareOrdinal(key, cur.Item.Key);
                if (cmp == 0) { cur.Item.Value = value; return; }     // replace
                if (cmp < 0)
                {
                    if (cur.Left == null) { cur.Left = node; return; }
                    cur = cur.Left;
                }
                else
                {
                    if (cur.Right == null) { cur.Right = node; return; }
                    cur = cur.Right;
                }
            }
        }

        public bool TryGet(string key, out TValue value)
        {
            var cur = _root;
            while (cur != null)
            {
                int cmp = string.CompareOrdinal(key, cur.Item.Key);
                if (cmp == 0) { value = cur.Item.Value; return true; }
                cur = (cmp < 0) ? cur.Left : cur.Right;
            }
            value = default(TValue);
            return false;
        }

        public IEnumerator<BSTValuePair<TValue>> GetEnumerator()
        {
            // In-order traversal using our own stack implemented with LinkedList
            var stack = new LinkedList<BSTNode<TValue>>(); // our custom LinkedList? We need System.Collections.Generic.Stack? Not allowed.
            // Use recursion-like loop with manual linked list as stack
            BSTNode<TValue> cur = _root;
            while (cur != null || !stack.IsEmpty())
            {
                while (cur != null)
                {
                    stack.AddLast(cur);
                    cur = cur.Left;
                }
                var top = stack.RemoveFirst(); // but our LinkedList has RemoveFirst() returning T? It returns T value. We stored BSTNode<TValue>.
                // Wait: our LinkedList<T> RemoveFirst() returns T. Good.
                var n = top; // n is BSTNode<TValue>
                yield return n.Item;
                cur = n.Right;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}