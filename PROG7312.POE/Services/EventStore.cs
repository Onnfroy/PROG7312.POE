using System;
using PROG7312.POE.Models;

namespace PROG7312.POE.Services
{
    public class EventStore
    {
        public System.Collections.Generic.SortedDictionary<DateTime, PROG7312.POE.DataStructures.LinkedList<EventItem>> ByDate
            = new System.Collections.Generic.SortedDictionary<DateTime, PROG7312.POE.DataStructures.LinkedList<EventItem>>();

        public System.Collections.Generic.HashSet<string> Categories
            = new System.Collections.Generic.HashSet<string>();

        public PROG7312.POE.DataStructures.Queue<EventItem> NewSubmissions
            = new PROG7312.POE.DataStructures.Queue<EventItem>();

        public PROG7312.POE.DataStructures.LinkedList<Guid> LastViewed
            = new PROG7312.POE.DataStructures.LinkedList<Guid>();

        public System.Collections.Generic.Dictionary<string, int> SearchCounts
            = new System.Collections.Generic.Dictionary<string, int>();

        // NEW: remember the most recently-triggered search key
        private string _lastSearchKey;
        public string LastSearchKey => _lastSearchKey;

        public void Add(EventItem e)
        {
            PROG7312.POE.DataStructures.LinkedList<EventItem> bucket;
            if (!ByDate.TryGetValue(e.Date.Date, out bucket))
            {
                bucket = new PROG7312.POE.DataStructures.LinkedList<EventItem>();
                ByDate[e.Date.Date] = bucket;
            }
            bucket.AddLast(e);
            Categories.Add(e.Category.ToString());
        }

        public PROG7312.POE.DataStructures.LinkedList<EventItem> AllFlat()
        {
            var result = new PROG7312.POE.DataStructures.LinkedList<EventItem>();
            foreach (var kv in ByDate)
                foreach (var e in kv.Value)
                    result.AddLast(e);
            return result;
        }

        // ---- Recommendation tracking (only set when user acts) ----
        public void RecordSearchTitle(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return;
            var key = "title:" + term.Trim().ToLowerInvariant();
            _lastSearchKey = key;
            if (!SearchCounts.ContainsKey(key)) SearchCounts[key] = 0;
            SearchCounts[key]++;
        }

        public void RecordSearchCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return;
            var key = "cat:" + category.Trim();
            _lastSearchKey = key;
            if (!SearchCounts.ContainsKey(key)) SearchCounts[key] = 0;
            SearchCounts[key]++;
        }

        public void RecordSearchDate(DateTime date)
        {
            var key = "date:" + date.Date.ToString("yyyy-MM-dd");
            _lastSearchKey = key;
            if (!SearchCounts.ContainsKey(key)) SearchCounts[key] = 0;
            SearchCounts[key]++;
        }

        public string TopSearchKey()
        {
            string topKey = null;
            var topCount = -1;
            foreach (var kv in SearchCounts)
                if (kv.Value > topCount) { topCount = kv.Value; topKey = kv.Key; }
            return topKey;
        }

        public string RecommendationText()
        {
            // Prefer LAST user action; if null, fall back to most frequent
            var key = _lastSearchKey ?? TopSearchKey();
            if (key == null) return "Recommendations: try filtering by Category, Date, or Title";

            if (key.StartsWith("cat:")) return "Recommendations: more in category " + key.Substring(4);
            if (key.StartsWith("date:")) return "Recommendations: events around " + key.Substring(5);
            if (key.StartsWith("title:")) return "Recommendations: titles matching '" + key.Substring(6) + "'";
            return "Recommendations: —";
        }

        public void ClearSearchCounts()
        {
            SearchCounts.Clear();
            _lastSearchKey = null;
        }

        public void PushViewed(Guid id) { LastViewed.AddLast(id); }

        public Guid? PopViewed()
        {
            Guid last = default(Guid);
            var found = false;
            foreach (var v in LastViewed) { last = v; found = true; }
            if (!found) return null;

            var rebuilt = new PROG7312.POE.DataStructures.LinkedList<Guid>();
            foreach (var v in LastViewed)
            {
                if (v.Equals(last)) break;
                rebuilt.AddLast(v);
            }
            LastViewed = rebuilt;
            return last;
        }

        // Helper used by UI
        public int PendingSubmissionsCount() => NewSubmissions.Count;
        public bool TryGet(Guid id, out EventItem item)
        {
            foreach (var e in AllFlat())
                if (e.Id == id) { item = e; return true; }
            item = null; return false;
        }
    }
}