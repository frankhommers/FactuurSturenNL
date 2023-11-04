using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Converters;

internal class FloatAsStringConverter : JsonConverter<float>
{
  public override float Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    JsonTokenType token = reader.TokenType;

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
    float data,
    JsonSerializerOptions options)
  {
    writer.WriteStringValue(data.ToString(CultureInfo.InvariantCulture));
  }
}