using System;
using PROG7312.POE.Models;

namespace PROG7312.POE.Models
{
    // Priority used by the heap (lower number = higher priority)
    public enum RequestPriority { Critical = 1, High = 2, Normal = 3, Low = 4 }

    public enum RequestStatus { Submitted, InProgress, Resolved }

    public class ServiceRequest
    {
        public Guid Id { get; } = Guid.NewGuid();                 // Unique identifier
        public string CitizenName { get; set; }                   // Who reported
        public IssueCategory Category { get; set; }               // Reuse existing enum
        public RequestPriority Priority { get; set; }             // For scheduling
        public RequestStatus Status { get; set; }                 // Lifecycle
        public string Location { get; set; }                      // e.g., "Ward 4"
        public DateTime CreatedAt { get; } = DateTime.Now;        // Created time
        public DateTime DueDate { get; set; }                     // Target completion

        public ServiceRequest(string name, IssueCategory cat, RequestPriority prio,
                              string location, DateTime due, RequestStatus status = RequestStatus.Submitted)
        {
            CitizenName = name;
            Category = cat;
            Priority = prio;
            Location = location;
            DueDate = due;
            Status = status;
        }
    }
}