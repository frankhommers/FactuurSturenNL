using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Converters;

internal class NullableDateOnlyAsStringConverter : JsonConverter<DateOnly?>
{
  public override DateOnly? Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    JsonTokenType token = reader.TokenType;

    if (token == JsonTokenType.Null) return null;

    if (token == JsonTokenType.String)
    {
      string s = reader.GetString() ?? string.Empty;
      if (string.IsNullOrWhiteSpace(s)) return null;
      if (!DateOnly.TryParse(s, CultureInfo.InvariantCulture, out DateOnly result))
        throw new FormatException("Expected date");
      return result;
    }

    throw new FormatException("Expected string");
  }

  public override void Write(
    Utf8JsonWriter writer,
    DateOnly? data,
    JsonSerializerOptions options)
  {
    if (data == null) writer.WriteNullValue();
    else
      writer.WriteStringValue(data.Value.ToString(CultureInfo.InvariantCulture));
  }
}