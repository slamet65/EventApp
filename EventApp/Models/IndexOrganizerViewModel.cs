using Newtonsoft.Json;
namespace EventApp.Models
{
    public class IndexOrganizerViewModel
    {
        [JsonProperty("data")]
        public IEnumerable<Organizer> Data { get; set; }
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }
}
