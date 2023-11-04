using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model
{
    public class Reference
    {
        [JsonPropertyName("line1")]
        public string Line1 { get; set; }
        [JsonPropertyName("line2")]
        public string Line2 { get; set; }
        [JsonPropertyName("line3")]
        public string Line3 { get; set; }
    }
}