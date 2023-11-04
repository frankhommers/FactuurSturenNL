using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

public class TaxItem
{
  [JsonPropertyName("rate")] public int Rate { get; set; }

  [JsonPropertyName("sum")] public string Sum { get; set; }

  [JsonPropertyName("sumof")] public string SumOf { get; set; }
}