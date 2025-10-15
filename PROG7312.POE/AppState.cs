using System;                           // DateTime
using PROG7312.POE.DataStructures;      // Queue<T> (your custom)
using PROG7312.POE.Services;            // IssueRepository, EventStore
using PROG7312.POE.Models;              // EventItem, EventCategory

namespace PROG7312.POE
{
    /// <summary>
    /// Global, in-memory state for the app (no DB).
    /// Holds issues (Part 1), engagement messages, and events (Part 2).
    /// </summary>
    public static class AppState
    {
        // ===== Part 1 =====
        public static readonly IssueRepository Issues = new IssueRepository();      // Store user-reported issues.
        public static readonly Queue<string> EngagementQueue = new Queue<string>(); // Rotating “thanks” messages.

        // ===== Part 2 =====
        public static readonly EventStore Events = new EventStore();                // All events + DS (SortedDictionary, HashSet, Queue, Stack).

        // Static ctor runs once when AppState is first referenced.
        static AppState()
        {
            // Seed engagement messages (Part 1).
            EngagementQueue.Enqueue("Thanks! Your report helps improve services.");
            EngagementQueue.Enqueue("Got it. We’ve logged your report.");
            EngagementQueue.Enqueue("You’re awesome for reporting this. Thank you.");
            EngagementQueue.Enqueue("Report received. We’ll keep you posted.");
        }

        /// <summary>
        /// One-time data population for Part 2 (15+ events).
        /// Safe to call multiple times (no duplicate load).
        /// </summary>
        public static void SeedEventsIfEmpty()
        {
            if (Events.ByDate.Count > 0) return; // already seeded

            var today = DateTime.Today;

            AddEv("Water Interruption Notice", today.AddDays(1), "Ward 4", EventCategory.Water, "Scheduled pipe maintenance 09:00–15:00.");
            AddEv("Road Resurfacing", today.AddDays(2), "Main Rd, Claremont", EventCategory.Roads, "Expect delays 08:00–17:00.");
            AddEv("Community Clean-up", today.AddDays(3), "Green Point Park", EventCategory.Community, "Bring gloves and bags.");
            AddEv("Safety Awareness Talk", today.AddDays(4), "Civic Centre Hall", EventCategory.PublicSafety, "Metro Police Q&A.");
            AddEv("Power Maintenance", today.AddDays(5), "Kenilworth", EventCategory.Electricity, "Load segment test 10:00–12:00.");
            AddEv("Library Story Time", today.AddDays(6), "Sea Point Library", EventCategory.Culture, "Kids 4–8.");
            AddEv("High School Info Session", today.AddDays(7), "Pinelands High", EventCategory.Education, "Parents welcome.");
            AddEv("Sports Day", today.AddDays(8), "Athlone Stadium", EventCategory.Sport, "Open community sports.");
            AddEv("Waste Sorting Demo", today.AddDays(9), "Muizenberg Beach", EventCategory.Sanitation, "Recycling basics.");
            AddEv("Substation Upgrade", today.AddDays(10), "Rondebosch", EventCategory.Electricity, "Limited outages.");
            AddEv("Bridge Repair Update", today.AddDays(11), "M5 Overpass", EventCategory.Roads, "Night work only.");
            AddEv("Water Tanker Schedule", today.AddDays(12), "Khayelitsha", EventCategory.Water, "Temporary supply.");
            AddEv("Neighbourhood Watch Meet", today.AddDays(13), "Observatory Hall", EventCategory.PublicSafety, "Patrol sign-ups.");
            AddEv("Cultural Festival", today.AddDays(14), "Company’s Garden", EventCategory.Culture, "Food & music.");
            AddEv("Career Expo", today.AddDays(15), "CTICC", EventCategory.Education, "Tertiary booths.");
        }

        // Helper to create and add an event into the EventStore structures.
        private static void AddEv(string title, DateTime date, string location, EventCategory category, string description)
        {
            Events.Add(new EventItem(title, date, location, category, description));
        }
    }
}