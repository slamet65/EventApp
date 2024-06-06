using System.Collections.Specialized;

namespace EventApp.Models
{
    public class ApiError
    {
        public string? message { get; set; }
        public Dictionary<string, List<string>>? errors { get; set; }
        public int? status_code { get; set; }
        public string? error { get; set; }
    }
}
