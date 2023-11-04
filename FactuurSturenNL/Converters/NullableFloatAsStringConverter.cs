using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Converters;

internal class NullableFloatAsStringConverter : JsonConverter<float?>
{
  public override float? Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    JsonTokenType token = reader.TokenType;

    if (token == JsonTokenType.Null) return null;

    if (token == JsonTokenType.String)
    {
      string s = reader.GetString() ?? "0";
      if (!float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
        throw new FormatException("Expected float");
      return result;
    }

    throw new FormatException("Expected string");
  }

  public override void Write(
    Utf8JsonWriter writer,
    float? data,
    JsonSerializerOptions options)
  {
    if (data == null) writer.WriteNullValue();
    else
      writer.WriteStringValue(data.Value.ToString(CultureInfo.InvariantCulture));
  }
}