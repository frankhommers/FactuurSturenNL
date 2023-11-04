using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

[JsonConverter(typeof(JsonStringEnumMemberConverter))] // This custom converter was placed in a system namespace.
public enum Language
{
  [EnumMember(Value = "default")] Default,
  [EnumMember(Value = "nl")] Dutch,
  [EnumMember(Value = "en")] English,
  [EnumMember(Value = "de")] German,
  [EnumMember(Value = "fr")] French,
  [EnumMember(Value = "es")] Spanish
}