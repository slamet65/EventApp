using Newtonsoft.Json;

namespace EventApp.Models
{
    public class Event
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventName")]
        public string EventName { get; set; }

        [JsonProperty("eventDate")]
        public string EventDate { get; set; }

        [JsonProperty("organizer")]
        public Organizer Organizer { get; set; }
    }

    public class CreateEvent
    {
        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventName")]
        public string EventName { get; set; }

        [JsonProperty("eventDate")]
        public string EventDate { get; set; }

        [JsonProperty("organizerId")]
        public int OrganizerId { get; set; }
        public IEnumerable<Organizer> ReffOrganizer { get; set; }

    }

    public class EditEvent
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventName")]
        public string EventName { get; set; }

        [JsonProperty("eventDate")]
        public string EventDate { get; set; }

        [JsonProperty("organizerId")]
        public int OrganizerId { get; set; }
        public IEnumerable<Organizer> ReffOrganizer { get; set; }

    }
}
