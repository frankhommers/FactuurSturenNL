using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

public class Product
{
  [JsonPropertyName("Id")] public int Id { get; set; }
  [JsonPropertyName("Code")] public string Code { get; set; }
  [JsonPropertyName("Name")] public string Name { get; set; }
  [JsonPropertyName("Price")] public double Price { get; set; }
  [JsonPropertyName("Taxes")] public int Taxes { get; set; }
  [JsonPropertyName("Priceintax")] public double Priceintax { get; set; }
}