using System.Text.Json;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Converters;

internal class BoolAsString0And1Converter : JsonConverter<bool>
{
  public override bool Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    JsonTokenType token = reader.TokenType;

    if (token == JsonTokenType.String)
    {
      string s = reader.GetString() ?? "";
      return s.Trim() switch
      {
        "1" => true,
        "0" => false
      };
    }

    throw new FormatException("Expected string");
  }

  public override void Write(
    Utf8JsonWriter writer,
    bool data,
    JsonSerializerOptions options)
  {
    writer.WriteStringValue(data ? "1" : "0");
  }
}