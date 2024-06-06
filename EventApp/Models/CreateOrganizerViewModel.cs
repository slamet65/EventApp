using Newtonsoft.Json;

namespace EventApp.Models
{
    public class CreateOrganizerViewModel
    {
        [JsonProperty("organizerName")]
        public string OrganizerName { get; set; }

        [JsonProperty("imageLocation")]
        public string ImageLocation { get; set; }
    }
}
