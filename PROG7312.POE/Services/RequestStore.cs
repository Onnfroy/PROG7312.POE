using System;
using System.Collections.Generic;              // For Dictionary / HashSet (allowed)
using PROG7312.POE.DataStructures;            // BST<T>, MinPriorityQueue<T>, Graph<T>, LinkedList<T>
using PROG7312.POE.Models;                   // ServiceRequest, RequestPriority, RequestStatus, IssueCategory

namespace PROG7312.POE.Services
{
    // Public so AppState.Requests (public) is valid.
    public sealed class RequestStore
    {
        // Primary index: request Id (string) -> ServiceRequest (BST implementation)
        public BST<ServiceRequest> ById = new BST<ServiceRequest>();

        // Min-priority queue for "next to service"
        public MinPriorityQueue<ServiceRequest> MinHeap;

        // Graph of depots/wards (for BFS route preview)
        public Graph<string> Routes = new Graph<string>();

        public RequestStore()
        {
            // Compare by (Priority, DueDate)
            MinHeap = new MinPriorityQueue<ServiceRequest>((a, b) =>
            {
                // lower enum value = higher priority
                int pc = ((int)a.Priority).CompareTo((int)b.Priority);
                if (pc != 0) return pc;
                // earlier due date wins
                return a.DueDate.CompareTo(b.DueDate);
            });

            // Simple undirected network of depots and wards
            Routes.AddEdge("Depot A", "Ward 1");
            Routes.AddEdge("Ward 1", "Ward 2");
            Routes.AddEdge("Ward 2", "Ward 3");
            Routes.AddEdge("Depot B", "Ward 3");
            Routes.AddEdge("Ward 3", "Ward 4");
            Routes.AddEdge("Ward 4", "Ward 5");
        }

        /// <summary>Add a request into all relevant structures (BST + heap).</summary>
        public void Add(ServiceRequest r)
        {
            if (r == null) throw new ArgumentNullException("r");
            // Id as string is our BST key
            ById.Insert(r.Id.ToString(), r);
            MinHeap.Enqueue(r);
        }

        /// <summary>Lookup by ID string using the BST.</summary>
        public bool TryGetById(string idText, out ServiceRequest req)
        {
            if (string.IsNullOrWhiteSpace(idText))
            {
                req = null;
                return false;
            }
            return ById.TryGet(idText.Trim(), out req);
        }

        /// <summary>Peek at the next request to service (min-heap) without removing it.</summary>
        public bool TryPeekNext(out ServiceRequest req)
        {
            try
            {
                req = MinHeap.PeekMin();
                return true;
            }
            catch
            {
                req = null;
                return false;
            }
        }

        /// <summary>Pop the next request to service (min-heap).</summary>
        public bool TryPopNext(out ServiceRequest req)
        {
            try
            {
                req = MinHeap.DequeueMin();
                return true;
            }
            catch
            {
                req = null;
                return false;
            }
        }

        /// <summary>Get a linked-list path between two nodes in the routes graph (BFS shortest path).</summary>
        public PROG7312.POE.DataStructures.LinkedList<string> ShortestPath(string from, string to)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return new PROG7312.POE.DataStructures.LinkedList<string>();

            return Routes.BfsShortestPath(from.Trim(), to.Trim());
        }
    }
}