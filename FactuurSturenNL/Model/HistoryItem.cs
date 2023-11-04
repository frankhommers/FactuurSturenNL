using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

public class HistoryItem
{
  [JsonPropertyName("date")] public string Date { get; set; }

  [JsonPropertyName("time")] public string Time { get; set; }

  [JsonPropertyName("description")] public string Description { get; set; }

  [JsonPropertyName("amount")] public double Amount { get; set; }
}