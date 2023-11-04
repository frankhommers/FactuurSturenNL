using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Converters;

internal class IntAsStringConverter : JsonConverter<int>
{
  public override int Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    JsonTokenType token = reader.TokenType;


    if (token == JsonTokenType.String)
    {
      string s = reader.GetString() ?? "0";
      if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
        throw new FormatException("Expected integer");
      return result;
    }

    throw new FormatException("Expected string");
  }

  public override void Write(
    Utf8JsonWriter writer,
    int data,
    JsonSerializerOptions options)
  {
    writer.WriteStringValue(data.ToString(CultureInfo.InvariantCulture));
  }
}