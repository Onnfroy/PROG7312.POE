using System;

namespace PROG7312.POE.Models
{
    public enum EventCategory
    {
        Community, Sanitation, Roads, Water, Electricity, PublicSafety, Sport, Culture, Education, Other
    }

    public class EventItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public EventCategory Category { get; set; }
        public string Description { get; set; }

        public EventItem(string title, DateTime date, string location, EventCategory category, string description)
        {
            Title = title;
            Date = date;
            Location = location;
            Category = category;
            Description = description;
        }
    }
}