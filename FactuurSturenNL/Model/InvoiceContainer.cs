using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

public class InvoiceContainer
{
  [JsonPropertyName("invoice")] public Invoice? Invoice { get; set; }
}