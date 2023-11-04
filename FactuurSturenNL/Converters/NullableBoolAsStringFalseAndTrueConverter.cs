using System.Text.Json;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Converters;

internal class NullableBoolAsStringFalseAndTrueConverter : JsonConverter<bool?>
{
  public override bool? Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    JsonTokenType token = reader.TokenType;

    if (token == JsonTokenType.Null) return null;
    if (token == JsonTokenType.String)
    {
      string s = reader.GetString() ?? "";
      return s.Trim().ToLower() switch
      {
        "true" => true,
        "false" => false,
        _ => null
      };
    }

    throw new FormatException("Expected string");
  }

  public override void Write(
    Utf8JsonWriter writer,
    bool? data,
    JsonSerializerOptions options)
  {
    if (data == null) writer.WriteNullValue();
    else
      writer.WriteStringValue(data.Value ? "1" : "0");
  }
}