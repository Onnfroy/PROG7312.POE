using System;
using System.Collections.Generic; // HashSet, Dictionary (allowed)
using PROG7312.POE.DataStructures; // your LinkedList<T>

namespace PROG7312.POE.DataStructures
{
    public sealed class Graph<T>
    {
        // adjacency list: vertex -> neighbors (custom linked list)
        private readonly Dictionary<T, LinkedList<T>> _adj = new Dictionary<T, LinkedList<T>>();

        public void AddVertex(T v)
        {
            if (!_adj.ContainsKey(v)) _adj[v] = new LinkedList<T>();
        }

        public void AddEdge(T from, T to, bool undirected = true)
        {
            AddVertex(from);
            AddVertex(to);
            _adj[from].AddLast(to);
            if (undirected) _adj[to].AddLast(from);
        }

        public LinkedList<T> BfsShortestPath(T start, T goal)
        {
            var q = new Queue<T>();            // your custom queue
            var visited = new HashSet<T>();
            var parent = new Dictionary<T, T>();

            if (!_adj.ContainsKey(start) || !_adj.ContainsKey(goal))
                return new LinkedList<T>();

            q.Enqueue(start);
            visited.Add(start);

            bool found = false;
            while (!q.IsEmpty())
            {
                var u = q.Dequeue();
                if (object.Equals(u, goal)) { found = true; break; }

                foreach (var v in _adj[u])
                {
                    if (!visited.Contains(v))
                    {
                        visited.Add(v);
                        parent[v] = u;
                        q.Enqueue(v);
                    }
                }
            }

            var path = new LinkedList<T>();
            if (!found) return path;

            // Reconstruct path from goal to start via parent dict, then prepend by building string later
            T cur = goal;
            while (!object.Equals(cur, default(T)) && parent.ContainsKey(cur))
            {
                path.AddLast(cur);
                cur = parent[cur];
            }
            path.AddLast(start);
            // path is [goal..start] in reverse order; we'll display by prepending in UI
            return path;
        }
    }
}