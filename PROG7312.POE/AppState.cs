using PROG7312.POE.DataStructures;   // Queue<T>
using PROG7312.POE.Services;         // IssueRepository

namespace PROG7312.POE
{
    public static class AppState
    {
        public static readonly IssueRepository Issues = new IssueRepository();   // Store.
        public static readonly Queue<string> EngagementQueue = new Queue<string>(); // Messages.

        static AppState()
        {
            EngagementQueue.Enqueue("Thanks! Your report helps improve services.");
            EngagementQueue.Enqueue("Got it. We’ve logged your report.");
            EngagementQueue.Enqueue("You’re awesome for reporting this. Thank you.");
            EngagementQueue.Enqueue("Report received. We’ll keep you posted.");
        }
    }
}