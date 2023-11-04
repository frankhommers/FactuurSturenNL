using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FactuurSturenNL.Model;

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum CollectType
{
  [EnumMember(Value = "none")] None,
  [EnumMember(Value = "OOFF")] SingleDirectDebit,
  [EnumMember(Value = "FRST")] FirstCollection,
  [EnumMember(Value = "RCUR")] RecurringCollection
}