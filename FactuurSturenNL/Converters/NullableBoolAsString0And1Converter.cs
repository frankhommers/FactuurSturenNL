using System.Text.Json;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Converters;

internal class NullableBoolAsString0And1Converter : JsonConverter<bool?>
{
  public override bool? Read(
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
        "0" => false,
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