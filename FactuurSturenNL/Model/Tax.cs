using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

public class Tax
{
  [JsonPropertyName("percentage")] public int Percentage { get; set; }

  [JsonPropertyName("type")] public TaxRate Type { get; set; }

  [JsonPropertyName("default")] public bool Default { get; set; }

  [JsonPropertyName("country")] public string Country { get; set; }
}