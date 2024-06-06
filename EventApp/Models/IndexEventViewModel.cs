using Newtonsoft.Json;

namespace EventApp.Models
{
    public class IndexEventViewModel
    {
        [JsonProperty("data")]
        public List<Event> Data { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }
        public IEnumerable<Organizer>  ReffOrganizer {  get; set; }
        public int? selectedOrganizerId { get; set; }
    }
}
