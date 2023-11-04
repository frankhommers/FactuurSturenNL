using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

public class ClientContainer
{
  [JsonPropertyName("client")] public Client? Client { get; set; }
}