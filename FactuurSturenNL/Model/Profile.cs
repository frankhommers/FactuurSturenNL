using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

public class Profile
{
  [JsonPropertyName("id")] public string Id { get; set; }
  [JsonPropertyName("name")] public string Name { get; set; }
  [JsonPropertyName("default")] public string Default { get; set; }
}