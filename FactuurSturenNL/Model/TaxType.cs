using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

[JsonConverter(typeof(JsonStringEnumMemberConverter))] // This custom converter was placed in a system namespace.
public enum TaxType
{
  [EnumMember(Value = "none")] None,
  [EnumMember(Value = "intax")] IncludingTax,
  [EnumMember(Value = "extax")] ExcludingTax
}