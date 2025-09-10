using System;                                           // Guid, DateTime.
using PROG7312.POE.DataStructures;                      // Our LinkedList<T>.

namespace PROG7312.POE.Models
{
    public sealed class Issue
    {
        public Guid Id { get; }                         // Trackable id.
        public DateTime CreatedAt { get; }              // Timestamp.
        public string Location { get; }                 // Where the issue is.
        public IssueCategory Category { get; }          // Type.
        public string Description { get; }              // Details.
        public LinkedList<Attachment> Attachments { get; } // Files chain.

        public Issue(string location, IssueCategory category, string description)
        {
            Id = Guid.NewGuid();                        // New id.
            CreatedAt = DateTime.Now;                   // Now.
            Location = location;                        // Save.
            Category = category;                        // Save.
            Description = description;                  // Save.
            Attachments = new LinkedList<Attachment>(); // Start empty chain.
        }
    }
}