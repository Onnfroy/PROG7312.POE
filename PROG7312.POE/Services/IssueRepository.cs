using System;                                   // Guard clauses.
using PROG7312.POE.DataStructures;              // LinkedList<T>.
using PROG7312.POE.Models;                      // Issue.

namespace PROG7312.POE.Services
{
    public sealed class IssueRepository
    {
        private readonly LinkedList<Issue> _issues = new LinkedList<Issue>(); // Store.

        public void Add(Issue issue)
        {
            if (issue == null) throw new ArgumentNullException(nameof(issue));
            _issues.AddLast(issue);               // Append.
        }

        public LinkedList<Issue> GetAll() => _issues; // Expose chain for foreach.
        public int Count => _issues.Count;            // Quick count.
    }
}