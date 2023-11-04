using System.Text.Json;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Converters;

internal class BoolAsStringFalseAndTrueConverter : JsonConverter<bool>
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
      return s.Trim().ToLower() switch
      {
        "true" => true,
        "false" => false
      };
    }

    throw new FormatException("Expected string");
  }

  public override void Write(
    Utf8JsonWriter writer,
    bool data,
    JsonSerializerOptions options)
  {
      writer.WriteStringValue(data ? "true" : "false");
  }
}